using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerScript : Character
{
    private bool _nextAttackBuffed, _boostedStats;
    private float _cTimer;

    protected override void SetValues(Attributes model)
    {
        model.moveSpeed = 3.5f;
        model.maxHealth = 100;
        model.health = 100;
        model.attackRange = 1.0f;
        model.attack = 2;
        model.attackPerLevel = 1;
        model.radius = 0.5f;
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        if (_boostedStats)
        {
            if (_cTimer > 0)
            {
                _cTimer -= Time.deltaTime;
            }

            if (_cTimer <= 0)
            {
                _boostedStats = false;
                model.physDef -= 20;
                model.magDef -= 20;
                model.moveSpeed /= 1.1f;
            }
        }
        
    }
    
    protected override bool DealAutoDamage(Entity target)
    {
        bool didKill = target.ReceiveDamage(this, model.attack + (_nextAttackBuffed ? 10 : 0), 0, model.physPen, model.magPen, model.critChance, model.critMult);
        if (_nextAttackBuffed) _nextAttackBuffed = false;
        return didKill;
    }

    public override void SpellA()
    {
        _nextAttackBuffed = true;
    }

    public override void SpellB()
    {
        model.health = Math.Max(model.health + 50, model.maxHealth);
    }

    public override void SpellC()
    {
        model.physDef += 20;
        model.magDef += 20;
        model.moveSpeed *= 1.1f;
        _boostedStats = true;
        _cTimer = 5f;
    }
}
