using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;
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

    public static Character GetCharacterByID(int id)
    {
        return FindObjectsByType<Character>(FindObjectsSortMode.None).FirstOrDefault(character => character.GetPlayerID() == id);
    }

    public abstract void SpellA();
    public abstract void SpellB();
    public abstract void SpellC();

    protected override int GetGoldBounty()
    {
        return 150;
    }

    protected override int GetExpBounty()
    {
        return 60;
    }

    public void SetPlayerID(int id)
    {
        model.playerID = id;
    }

    public int GetPlayerID()
    {
        return model.playerID;
    }
    
    public void InitInventory() {
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            model.inventory.Add(i, new Item());
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<RealtimeView>();
    }

    protected override void Update()
    {
        if (model.playerID == realtime.clientID)
        {
            if (_deathTimer <= 0)
            {
                base.Update();
                            
                model.PassiveIncomeTimer += Time.deltaTime;
                while (model.PassiveIncomeTimer >= 1)
                {
                    model.exp += Attributes.PassiveExp;
                    model.golds += Attributes.PassiveGold;
                    --(model.PassiveIncomeTimer);
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
        if (item.IsCrafted()) {
            //do he have all needed to?
            foreach(string name in item.GetRecipe()) {
                uint emplacement = GetItemEmplacmentByName(name);
                if (emplacement == 6) return false;
                else if (nextEmptyEmplacement == 6) nextEmptyEmplacement = emplacement;
            }

            //let's convert them on the new item
            foreach(string name in item.GetRecipe()) {
                DropItem(GetItemEmplacmentByName(name), false);
            }
        } else {
            //Find an empty emplacement
            nextEmptyEmplacement = GetItemEmplacmentByName("Item");

            if(nextEmptyEmplacement == 6) return false;
        }

        //update player balance
        model.golds -= (int) item.GetCost();
        
        //update player stats
        model.attack += item.GetAttack();
        model.critChance += item.GetCritChance();
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
        model.health -= item.GetHealth();
        model.maxHealth -= item.GetHealth();

        if (isSelling) model.golds += (int) item.GetSellingCost();

        model.inventory[itemEmplacement] = new Item();
    }

    protected virtual void AttackLogic()
    {
        if (model.Target)
        {
            if (Vector3.Distance(transform.position, model.Target.transform.position) - model.radius - model.Target.GetRadius() <= model.attackRange)
            {
                // logique d'attaque
                agent.ResetPath();
                DealAutoDamage(model.Target);
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
                            if(movementIcon) Instantiate(movementIcon, hit.point + IconOffset, Quaternion.identity);
                            agent.SetDestination(hit.point);
                            model.Target = null;
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
}
