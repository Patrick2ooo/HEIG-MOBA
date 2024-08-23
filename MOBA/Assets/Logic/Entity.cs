using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : MonoBehaviour
{
    public ushort side;
    protected int PhysDef, MagDef;

    protected float Attack, AttackRange, Health, MaxHealth, WindUpTime, AttackTime, RecoveryTime;

    protected string Name;

    protected List<Character> LastHitters;

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

    public bool ReceiveDamage(Character hitter, float physDmg, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        if (Health == 0) return false; // in case it happens that the entity receive damage after its death for whatever reason
        // dégâts reçus = dégâts de base * (pen + (1-pen) * 100 / (def + 100))
        float phys = physDmg * (physPen + (1 - physPen) * 100 / (PhysDef + 100));
        Health = Math.Max(0, 
            Health - (critChance >= Random.Range(0, 1) ? phys * critMult : phys)
                   - magDmg * (magPen + (1 - magPen) * 100 / (MagDef + 100))
        );
        LastHitters.Add(hitter);
        return Health == 0; // true if dealing damage lands the killing blow
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Health == 0)
        {
            Character killer = LastHitters.Last();
            killer.Golds += GetGoldBounty();
            killer.Exp += GetExpBounty();
        }
    }
}
