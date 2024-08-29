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
    
    protected bool AcquireAnItem(Item item) {
        int nextEmptyEmplacement = -1;
        for(int i = 0; i < Attributes.NbInventorySlots; ++i) {
            if(!(Inventory == null) && Inventory[i] == null) {
                nextEmptyEmplacement = i;
                break;
            }
        }
        if (nextEmptyEmplacement == -1) return false;

        GameObject itemGO = Realtime.Instantiate(prefabName: item.GetName(), ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        itemGO.transform.SetParent(transform.Find("Inventory"));
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
        if (Inventory == null || Inventory[itemEmplacement] == null) return;

        /*
        if (Inventory[itemEmplacement].IsActivable()) REMOVE THE RIGHT ACTIVE BUTTON and all of that
        */

        model.attack -= Inventory[itemEmplacement].GetAttack();
        model.health -= Inventory[itemEmplacement].GetHealth();
        model.maxHealth -= Inventory[itemEmplacement].GetHealth();

        Transform parent = transform.Find("Inventory");
        GameObject itemGO = parent.Find(Inventory[itemEmplacement].GetName()+"(Clone)").gameObject;
        Destroy(itemGO);
        Inventory[itemEmplacement] = null;
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
            CravacheSevere item = gameObject.AddComponent<CravacheSevere>();
            item.init();
            AcquireAnItem(item);
           
           
        }
        if (model.level < 12 && model.exp > Levels[model.level])
        {
            LevelUp();
        }
    }
}
