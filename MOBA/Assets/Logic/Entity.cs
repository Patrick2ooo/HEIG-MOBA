using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public ushort side;
    protected int PhysDef, MagDef;

    protected float Attack, AttackRange, Health, MaxHealth, WindUpTime, AttackTime, RecoveryTime;

    protected string Name;

    public abstract int GetGoldBounty();
    public abstract int GetExpBounty();

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public float GetHealth()
    {
        return Health;
    }

    public float GetHealthPercent()
    {
        return Health / MaxHealth;
    }

    public bool ReceiveDamage(float phys, float mag)
    {
        Health = Math.Max(0, 
            Health - Math.Max(0, phys / (1 + PhysDef / 100f) )
                   - Math.Max(0, mag / (1 + MagDef / 100f) )
        );
        return Health <= 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
