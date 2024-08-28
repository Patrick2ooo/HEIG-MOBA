using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Scripts;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    
    protected Item[] Inventory;

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

    protected bool AcquireAnItem(String itemName) {
        int nextEmptyEmplacement = -1;
        for(int i = 0; i < Attributes.NbInventorySlots; ++i) {
            Debug.Log("Empacement "+ i + "is");
            if(Inventory[i] == null) {
                Debug.Log("Empacement "+ i + "is free.");
                nextEmptyEmplacement = i;
                break;
            }
        }
        Debug.Log("Empacement "+ nextEmptyEmplacement + "is empty.");
        if (nextEmptyEmplacement == -1) return false;

        GameObject itemGO = Realtime.Instantiate(prefabName: "Cravache Sévère", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        itemGO.transform.SetParent(transform.Find("Inventory"));
        Item item = itemGO.transform.Find(itemName).GetComponent<Item>();
        Debug.Log("Item game object done. Item:" + item.GetName());
        Inventory[nextEmptyEmplacement] = item;
        
        model.attack += item.GetAttack();
        model.health += item.GetHealth();
        model.maxHealth += item.GetHealth();

        /*
        if (item.IsActivable()) ADD AN ACTIVE BUTTON and all of that
        */

        return true;
    }

    protected void DropAnItem(int itemEmplacement) {
        /*model.attack -= Inventory[itemEmplacement].GetAttack();
        model.health -= Inventory[itemEmplacement].GetHealth();
        model.maxHealth -= Inventory[itemEmplacement].GetHealth();*/

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        //Inventory[itemEmplacement] = null;
    }
    
    void Start()
    {
        model.ExpTimer = 0;
        Inventory = new Item[Attributes.NbInventorySlots];
        for (int i = 0; i; i < Attributes.NbInventorySlots; ++int) Inventory[i] = null;
    }

    protected virtual void Update()
    {
        base.Update();
        model.ExpTimer += Time.deltaTime;

        //For test only
            AcquireAnItem("Cravache Sévère");

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
