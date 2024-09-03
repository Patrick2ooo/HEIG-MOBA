using System;
using UnityEngine;

public class PlayerScript : Character
{
    private bool _nextAttackBuffed, _boostedStats;
    private float _cTimer;

    protected override void SetValues(Attributes attributes)
    {
        attributes.moveSpeed = 3.5f;
        attributes.maxHealth = 100;
        attributes.health = 100;
        attributes.attackRange = 1.0f;
        attributes.attack = 1;
        attributes.attackPerLevel = 1;
        attributes.radius = 0.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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

    protected override void DealAutoDamage(Entity target)
    {
        damageManager.AddDamage(target, model.attack + (_nextAttackBuffed ? 10 : 0), 0, model.physPen, model.magPen, model.critChance, model.critMult);
        if (_nextAttackBuffed) _nextAttackBuffed = false;
    }

    public override void SpellA()
    {
        _nextAttackBuffed = true;
    }

    public override void SpellB()
    {
        model.health = Math.Min(model.health + 50, model.maxHealth);
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
