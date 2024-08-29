using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Scripts;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};

    protected virtual void DealAutoDamage(Entity target)
    {
        target.ReceiveDamage(this, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
    }

    public void SetTarget(Entity target)
    {
        model.Target = target;
    }

    protected void LevelUp()
    {
        ++(model.level);
        model.attack += model.attackPerLevel;
        model.physDef += model.physDefPerLevel;
        model.magDef += model.magDefPerLevel;
        model.health += model.healthPerLevel * GetHealthPercent();
        model.maxHealth += model.healthPerLevel;
    }

    public abstract void SpellA();
    public abstract void SpellB();
    public abstract void SpellC();
    public void InitInventory() {
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            model.inventory.Add(i, new Item());
        }
    }
    
    protected bool BuyItem(string itemName) {
        uint nextEmptyEmplacement = 6;
        for(uint i = 0; i < Attributes.NbInventorySlots; ++i) {
            if(model.inventory[i].GetName() == "Item") {
                nextEmptyEmplacement = i;
                break;
            }
        }

        if(nextEmptyEmplacement == 6) return false;

        Item item = Item.GetItemByName(itemName);
        if (model.golds - item.GetCost() < 0) return false;

        model.golds -= item.GetCost();
        
        model.attack += item.GetAttack();
        model.health += item.GetHealth();
        model.maxHealth += item.GetHealth();

        model.inventory[nextEmptyEmplacement] = item;
        /*
        if (item.IsActivable()) ADD AN ACTIVE BUTTON and all of that
        */

        return true;
    }

    protected void SellItem(uint itemEmplacement) {
        if (model.inventory[itemEmplacement].GetName() == "Item") return;

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        Item item = model.inventory[itemEmplacement];

        model.attack -= item.GetAttack();
        model.health -= item.GetHealth();
        model.maxHealth -= item.GetHealth();

        model.golds += item.GetSellingCost();

        model.inventory[itemEmplacement] = new Item();
    }
    
    void Start()
    {
        model.ExpTimer = 0;
    }

    protected virtual void Update()
    {
        base.Update();

        model.ExpTimer += Time.deltaTime;

        while (model.ExpTimer > 1)
        {
            ++(model.exp);
            --(model.ExpTimer);
        }

        if (model.level < 12 && model.exp > Levels[model.level])
        {
            LevelUp();
        }
    }
}
