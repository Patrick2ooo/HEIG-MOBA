using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;
using TMPro;
using Scripts;
using UnityEngine.EventSystems;

public abstract class Character : Entity
{
    private static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    
    public Camera mainCamera;
    public GameObject movementIcon;
    public GameObject deathScreen;
    public Vector3 playerBase;
    
    private const int MapLayer = 3, UILayer = 5, CharactersLayer = 6, ColliderLayer = 8;
    private static readonly Vector3 IconOffset = new(0, 0.1f, 0);
    private RealtimeView _view;
    private float _deathTimer;
    public Animator playerAnim;

    public abstract void SpellA();
    public abstract void SpellB();
    public abstract void SpellC();

    protected override void SetValues(Attributes model)
    {
        transform.position = playerBase;
    }

    public void AddExpGolds(int exp, int golds)
    {
        model.exp += exp;
        model.golds += golds;
    }

    protected override int GetGoldBounty()
    {
        return 150;
    }

    protected override int GetExpBounty()
    {
        return 60;
    }

    public string GetPlayerID()
    {
        return model.entityID;
    }
    
    public void InitInventory() {
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            model.inventory.Add(i, new Item(true));
        }
    }

    protected void Awake()
    {
        _view = GetComponent<RealtimeView>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    protected override void KillSelf() { }

    protected override void Update()
    {
        if (agent.isStopped)
        {
            //playerAnim.SetBool("isWalking", false);
        }
        if (model.entityID == "0" + realtime.clientID)
        {
            if (_deathTimer <= 0)
            {
                base.Update();

                if (model.recoveryTime > 0)
                {
                    model.recoveryTime -= Time.deltaTime;
                }
                
                model.PassiveIncomeTimer += Time.deltaTime;
                while (model.PassiveIncomeTimer >= 1)
                {
                    model.exp += Attributes.PassiveExp;
                    model.golds += Attributes.PassiveGold;
                    --(model.PassiveIncomeTimer);
                    GameObject.FindWithTag("goldLabel").GetComponent<TMP_Text>().text = "Gold: " + model.golds;
                }
    
                if (model.level < Attributes.MaxLevel && model.exp > Levels[model.level])
                {
                    LevelUp();
                }
    
                if (model.health <= 0)
                {
                    _deathTimer = 5;
                    deathScreen.SetActive(true);
                    Target = null;
                    agent.ResetPath();
                }
                
                CheckForScreenInteraction();
                AttackLogic();
            }
            else
            {
                _deathTimer -= Time.deltaTime;
                if (_deathTimer <= 0)
                {
                    deathScreen.SetActive(false);
                    model.health = model.maxHealth;
                    transform.position = playerBase;
                    model.windUpTime = 0;
                    model.attackTime = 0;
                    model.recoveryTime = 0;
                }
            }
        }
    }
    
    protected uint GetItemEmplacmentByName(string name) {
        uint emplacement = 6;
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            if(model.inventory[i].GetName() == name) {
                emplacement = i;
                break;
            }
        }

        return emplacement;
    }

    public bool BuyItem(string itemName) {
        uint nextEmptyEmplacement = 6;
        
        //Check player balance
        Item item = Item.GetItemByName(itemName);
        if (model.golds - item.GetCost() < 0) return false;

        //manage crafted items
        uint extraGoldNeeded = 0;
        if (item.IsCrafted()) {
            //do he have all needed to?
            foreach(string name in item.GetRecipe()) {
                uint emplacement = GetItemEmplacmentByName(name);
                if (emplacement == 6) extraGoldNeeded += Item.GetItemByName(name).GetCost();
                else if (nextEmptyEmplacement == 6) nextEmptyEmplacement = emplacement;
            }

            if(extraGoldNeeded + item.GetCost() > model.golds) return false;

            //let's convert them on the new item
            foreach(string name in item.GetRecipe()) {
                uint emplacement = GetItemEmplacmentByName(name);
                if (emplacement != 6) DropItem(emplacement, false);
            }
        }

        //Find an empty emplacement
        if(nextEmptyEmplacement == 6) {
            nextEmptyEmplacement = GetItemEmplacmentByName("Item");

            if(nextEmptyEmplacement == 6) return false;
        }

        //update player balance
        model.golds -= (int) (item.GetCost() + extraGoldNeeded);
        
        //update player stats
        model.attack += item.GetAttack();
        model.critChance += item.GetCritChance();
        model.critMult += item.GetCritDamage();
        model.health += item.GetHealth();
        model.maxHealth += item.GetHealth();

        model.inventory[nextEmptyEmplacement] = item;
        /*
        if (item.IsActivable()) ADD AN ACTIVE BUTTON and all of that
        */

        return true;
    }

    public void DropItem(uint itemEmplacement, bool isSelling) {
        if (model.inventory[itemEmplacement].GetName() == "Item") return;

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        Item item = model.inventory[itemEmplacement];

        model.attack -= item.GetAttack();
        model.critChance -= item.GetCritChance();
        model.critMult -= item.GetCritDamage();
        model.health -= item.GetHealth();
        model.maxHealth -= item.GetHealth();

        if (isSelling) model.golds += (int) item.GetSellingCost();

        model.inventory[itemEmplacement] = new Item(true);
    }

    protected virtual void AttackLogic()
    {
        if (model.Target)
        {
            if(model.recoveryTime > 0) return;
            if (Vector3.Distance(transform.position, model.Target.transform.position) - model.radius - model.Target.GetRadius() <= model.attackRange)
            {
                // logique d'attaque
                agent.ResetPath();
                if (model.windUpTime <= 0)
                {
                    model.windUpTime = WindUpDuration;
                }
                else
                {
                    model.windUpTime -= Time.deltaTime;
                    if (model.windUpTime <= 0)
                    {
                        DealAutoDamage(model.Target);
                        model.attackTime += model.windUpTime;
                    }
                }
            }
            else
            {
                agent.SetDestination(model.Target.transform.position);
            }
        }
    }

    private void Start()
    {
        if (_view.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }
    }

    private void LevelUp()
    {
        ++(model.level);
        model.attack += model.attackPerLevel;
        model.physDef += model.physDefPerLevel;
        model.magDef += model.magDefPerLevel;
        model.health += model.healthPerLevel * GetHealthPercent();
        model.maxHealth += model.healthPerLevel;
    }

    private void CheckForScreenInteraction()
    {
        if (Input.GetMouseButton(0))
        {
            if(model.attackTime > 0) return;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, layerMask:~(1 << ColliderLayer)))
            {
                var eventData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                if (results.Count(r => r.gameObject.layer == UILayer) == 0)
                {
                    switch (hit.collider.gameObject.layer)
                    {
                        case MapLayer:
                            //playerAnim.SetBool("isWalking", true);
                            if(movementIcon) Instantiate(movementIcon, hit.point + IconOffset, Quaternion.identity);
                            agent.SetDestination(hit.point);
                            model.Target = null;
                            model.recoveryTime = 0;
                            model.windUpTime = 0;
                            break;
                        case CharactersLayer:
                            model.Target = hit.collider.gameObject.GetComponent<Entity>();
                            Vector3 pos = model.Target.transform.position;
                            if (model.Target.GetSide() == model.side)
                            {
                                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, ~(1 << CharactersLayer));
                                pos = hit.point;
                                model.Target = null;
                            }
                            agent.SetDestination(pos);
                            break;
                    }
                }
            }
        }
    }
    
    protected override void DealAutoDamage(Entity target)
    {
        playerAnim.SetBool("isAttacking", true);
        damageManager.AddDamage(target, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
        playerAnim.SetBool("isAttacking", false);
    }

    public Item GetItem(uint slotId) {
        return model.inventory[slotId];
    }
    
    
    public Item[] GetInventory()
    {
        Item[] myItems = new Item[6];
        
        for(uint i = 0; i < 6; ++i) {
            myItems[i] = model.inventory[i];
        }
        
        return myItems;
    }

    public bool isAttacking()
    {
        if(model.windUpTime > 0)
            Debug.Log("WindUp");
        if(model.attackTime > 0)
            Debug.Log("attackTime");
        if(model.recoveryTime > 0)
            Debug.Log("recoveryTime");
        
        if (model.windUpTime > 0 || model.attackTime > 0 || model.recoveryTime > 0)
        {
            Debug.Log("isAttacking");
            return true;
        }
        else
        {
            
            Debug.Log("isNotAttacking");
            return false;
        }
    }
    
}
