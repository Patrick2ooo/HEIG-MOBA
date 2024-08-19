using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity
{
    protected static readonly int[] Levels = {0, 10, 30, 60, 100, 140, 190, 250, 320, 400, 490, 570};
    protected const int MaxLevel = 12;
    protected int Level = 1, Exp = 0, PhysDefPerLevel, MagDefPerLevel;
    private int _golds;

    protected float AttackPerLevel, HealthPerLevel, ExpTimer;

    protected Entity Target;

    protected void DealAutoDamage(Entity target)
    {
        if (target.ReceiveDamage(Attack, 0))
        {
            _golds += target.GetGoldBounty();
            Exp += target.GetExpBounty();
        }
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
