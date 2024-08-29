using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Entity : RealtimeComponent<Attributes>{

    public abstract int GetGoldBounty();
    public abstract int GetExpBounty();

    public float radius;
    public NavMeshAgent agent;
    protected Entity Target;

    protected virtual bool DealAutoDamage(Entity target)
    {
        return target.ReceiveDamage(this, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
    }

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
                currentModel.health = 1;
                currentModel.moveSpeed = 3.5f;
            }

            currentModel.healthDidChange += UpdateHealth;
            currentModel.moveSpeedDidChange += UpdateMoveSpeed;
        }
    }

    protected virtual void UpdateHealth(Attributes updated, float health)
    {
        if (health <= 0)
        {
            Realtime.Destroy(gameObject);
        }
    }

    protected virtual void UpdateMoveSpeed(Attributes updated, float speed)
    {
        agent.speed = speed;
    }

    public bool ReceiveDamage(Entity hitter, float physDmg, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        if (model.health == 0) return false; // in case it happens that the entity receive damage after its death for whatever reason
        // dégâts reçus = dégâts de base * (pen + (1-pen) * 100 / (def + 100))
        float phys = physDmg * (physPen + (1 - physPen) * 100 / (model.physDef + 100));
        model.health = Math.Max(0, 
            model.health - (critChance >= Random.Range(0, 1) ? phys * critMult : phys)
                   - magDmg * (magPen + (1 - magPen) * 100 / (model.magDef + 100))
        );
        //LastHitters.Add(hitter);
        return model.health == 0; // true if dealing damage lands the killing blow
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (model.health == 0)
        {
            Character killer = model.LastHitters.Last();
            killer.model.golds += GetGoldBounty();
            killer.model.exp += GetExpBounty();
        }
    }

    public void setSide(ushort side)
    {
        model.side = side;
    }
}
