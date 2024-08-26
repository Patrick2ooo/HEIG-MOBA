using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    protected const int MaxLevel = 12, PassiveGold = 1, PassiveExp = 1;
    protected int Level = 1, PhysDefPerLevel, MagDefPerLevel;
    public int Golds, Exp;

    protected float AttackPerLevel, HealthPerLevel, HealthRegen, HealthRegenPerLevel, ExpTimer, AttackSpeed, Haste, CritChance, CritMult = 1.5f, PhysPen, MagPen, PhysLifeSteal, MagLifeSteal, Mana, ManaPerLevel, ManaRegen, ManaRegenPerLevel, MoveSpeed;

    protected int Cores, PlayerId;

    protected Item[] Items;

    protected Entity Target;

    protected void DealAutoDamage(Entity target)
    {
        target.ReceiveDamage(this, Attack, 0, PhysPen, MagPen, CritChance, CritMult);
    }

    public void SetTarget(Entity target)
    {
        Target = target;
    }

    protected void LevelUp()
    {
        ++Level;
        Attack += AttackPerLevel;
        PhysDef += PhysDefPerLevel;
        MagDef += MagDefPerLevel;
        Health += HealthPerLevel * GetHealthPercent();
        MaxHealth += HealthPerLevel;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ExpTimer = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        base.Update();
        ExpTimer += Time.deltaTime;
        while (ExpTimer > 1)
        {
            ++Exp;
            --ExpTimer;
        }
        if (Level < 12 && Exp > Levels[Level])
        {
            LevelUp();
        }
    }
}
