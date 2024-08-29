using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Scripts;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    
    protected Item[] Inventory = new Item[Attributes.NbInventorySlots];

    protected void DealAutoDamage(Entity target)
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
    
    protected bool AcquireAnItem(string itemName) {
        if (model.inventory == null) model.inventory = new string[Attributes.NbInventorySlots];
        int nextEmptyEmplacement = -1;
        for(int i = 0; i < Attributes.NbInventorySlots; ++i) {
            if(mode.inventory[i] == null || model.inventory[i] == "") {
                nextEmptyEmplacement = i;
                break;
            }
        }
        if (nextEmptyEmplacement == -1) return false;

        model.inventory[nextEmptyEmplacement] = Item.GetItemByName(itemName).GetName();
        
        model.attack += Item.GetItemByName(itemName).GetAttack();
        model.health += Item.GetItemByName(itemName).GetHealth();
        model.maxHealth += Item.GetItemByName(itemName).GetHealth();

        /*
        if (item.IsActivable()) ADD AN ACTIVE BUTTON and all of that
        */

        return true;
    }

    protected void DropAnItem(int itemEmplacement) {
        if (model.inventory == null || model.inventory[itemEmplacement] == null || model.inventory[itemEmplacement] == "") return;

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        string itemName = model.inventory[itemEmplacement];
        
        model.attack -= Item.GetItemByName(itemName).GetAttack();
        model.health -= Item.GetItemByName(itemName).GetHealth();
        model.maxHealth -= Item.GetItemByName(itemName).GetHealth();

        model.inventory[itemEmplacement] = null;
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
            
            //For test only
            AcquireAnItem("Cravache Sévère");
        }

        if (model.level < 12 && model.exp > Levels[model.level])
        {
            LevelUp();
        }
    }
}
