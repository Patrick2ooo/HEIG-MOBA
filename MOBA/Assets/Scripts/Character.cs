using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    
    protected Item[] Items;

    public void SetTarget(Entity target)
    {
        Target = target;
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
    
    // Start is called before the first frame update
    void Start()
    {
        model.ExpTimer = 0;
    }

    // Update is called once per frame
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
