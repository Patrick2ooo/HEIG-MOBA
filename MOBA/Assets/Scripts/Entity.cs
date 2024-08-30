using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Entity : RealtimeComponent<Attributes>{
    
    public NavMeshAgent agent;
    protected Entity Target;

    public abstract int GetGoldBounty();
    public abstract int GetExpBounty();
    protected abstract void SetValues(Attributes model);
    
    public float GetMaxHealth()
    {
        return model.maxHealth;
    }

    public float GetHealth()
    {
        return model.health;
    }

    public float GetHealthPercent()
    {
        return model.health / model.maxHealth;
    }

    public ushort GetSide() {
        return model.side;
    }

    public float GetRadius()
    {
        return model.radius;
    }

    public void SetTarget(Entity target)
    {
        model.Target = target;
    }

    public void SetSide(ushort side)
    {
        model.side = side;
    }

    protected override void OnRealtimeModelReplaced(Attributes previousModel, Attributes currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);
        if (previousModel != null)
        {
            previousModel.healthDidChange -= UpdateHealth;
        }
        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                SetValues(currentModel);
            }

            currentModel.healthDidChange += UpdateHealth;
            currentModel.moveSpeedDidChange += UpdateMoveSpeed;
        }
    }
    
    protected virtual bool DealAutoDamage(Entity target)
    {
        return target.ReceiveDamage(this, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
    }
    
    protected virtual void UpdateHealth(Attributes updated, float health)
    {
        if (health <= 0)
        {
            Realtime.Destroy(gameObject);
        }
    }

    protected void UpdateMoveSpeed(Attributes updated, float speed)
    {
        agent.speed = speed;
    }

    public bool ReceiveDamage(Entity hitter, float physDmg, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        if (model.health <= 0) return false; // in case the entity receive damage after its death for whatever reason
        // dégâts reçus = dégâts de base * (pen + (1-pen) * 100 / (def + 100))
        float phys = physDmg * (physPen + (1 - physPen) * 100 / (model.physDef + 100));
        model.health = Math.Max(0, 
            model.health - (critChance >= Random.Range(0, 1) ? phys * critMult : phys)
                   - magDmg * (magPen + (1 - magPen) * 100 / (model.magDef + 100))
        );
        if (hitter.GetType() == typeof(Character))
        {
            model.LastHitters.Push((Character) hitter);
        }
        return model.health <= 0;
    }

    protected virtual void Update()
    {
        if (model.health == 0 && model.LastHitters.Count > 0)
        {
            Character killer = model.LastHitters.Peek();
            killer.model.golds += GetGoldBounty();
            killer.model.exp += GetExpBounty();
            model.LastHitters.Clear();
        }
        
        model.RegenTimer += Time.deltaTime;
        while (model.RegenTimer >= 1)
        {
            model.health = Math.Max(model.health + model.healthRegen, model.maxHealth);
            --(model.RegenTimer);
        }
    }
}
