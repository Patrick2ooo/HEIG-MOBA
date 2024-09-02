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

        //logic du joueur qui tire à modifier et discuter pour le mettre dans un spell
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                
                Vector3 direction = (targetPosition - transform.position).normalized;
                Vector3 spawnPosition = transform.position + direction * 1.0f;

                // Instantiate the projectile   
                GameObject proj = Realtime.Instantiate("Projectile", spawnPosition, Quaternion.LookRotation(targetPosition - transform.position), preventOwnershipTakeover: true, useInstance: _realtime);

                // Set the direction of the projectile
                ProjectileScript projScript = proj.GetComponent<ProjectileScript>();
                projScript.SetDirection((targetPosition - transform.position).normalized);
            }
        }*/

    }

    protected override void DealAutoDamage(Entity target)
    {
        manager.AddDamage(target, model.attack + (_nextAttackBuffed ? 10 : 0), 0, model.physPen, model.magPen, model.critChance, model.critMult);
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
