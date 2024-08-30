using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;
using Scripts;
using UnityEngine.EventSystems;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    
    public Camera mainCamera;
    public GameObject movementIcon;
    
    protected const int MapLayer = 3, UILayer = 5, CharactersLayer = 6, ColliderLayer = 8;
    protected static readonly Vector3 IconOffset = new(0, 0.1f, 0);
    protected RealtimeView _view;

    public abstract void SpellA();
    public abstract void SpellB();
    public abstract void SpellC();

    public override int GetGoldBounty()
    {
        return 150;
    }

    public override int GetExpBounty()
    {
        return 60;
    }
    
    public void InitInventory() {
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            model.inventory.Add(i, new Item());
        }
    }
    
    protected override void UpdateHealth(Attributes updated, float health)
    {
        if (health <= 0)
        {
            // à changer pour la gestion de la mort
            Realtime.Destroy(transform.parent.gameObject);
        }
    }

    protected void Awake()
    {
        _view = GetComponent<RealtimeView>();
    }

    protected virtual void Update()
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
        
        if (_view.isOwnedLocallyInHierarchy)
        {
            base.Update();
            CheckForScreenInteraction();
            AttackLogic();
        }
    }
    
    protected bool AcquireAnItem(string itemName) {
        uint nextEmptyEmplacement = 6;
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            if(model.inventory[i].GetName() == "Item") {
                nextEmptyEmplacement = i;
                break;
            }
        }

        if(nextEmptyEmplacement == 6) return false;

        Item item = Item.GetItemByName(itemName);
        model.inventory[nextEmptyEmplacement] = item;
        
        model.attack += item.GetAttack();
        model.health += item.GetHealth();
        model.maxHealth += item.GetHealth();

        /*
        if (item.IsActivable()) ADD AN ACTIVE BUTTON and all of that
        */

        return true;
    }

    protected void DropAnItem(uint itemEmplacement) {
        if (model.inventory[itemEmplacement].GetName() == "Item") return;

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        Item item = model.inventory[itemEmplacement];

        model.attack -= item.GetAttack();
        model.health -= item.GetHealth();
        model.maxHealth -= item.GetHealth();

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
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
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
