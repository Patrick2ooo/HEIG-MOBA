using System;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Entity : RealtimeComponent<Attributes>{
    
    public NavMeshAgent agent;
    public DamageManager manager;
    protected Entity Target;

    protected abstract int GetGoldBounty();
    protected abstract int GetExpBounty();
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
        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                SetValues(currentModel);
            }
            currentModel.moveSpeedDidChange += UpdateMoveSpeed;
        }
    }
    
    protected virtual void DealAutoDamage(Entity target)
    {
        manager.AddDamage(target, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
    }

    private void UpdateMoveSpeed(Attributes updated, float speed)
    {
        agent.speed = speed;
    }

    public void ReceiveDamage(int attackerID, float physDmg, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        if (model.health <= 0) return; // in case the entity receive damage after its death for whatever reason
        // dégâts reçus = dégâts de base * (pen + (1-pen) * 100 / (def + 100))
        float phys = physDmg * (physPen + (1 - physPen) * 100 / (model.physDef + 100));
        model.health = Math.Max(0, 
            model.health - (critChance >= Random.Range(0, 1) ? phys * critMult : phys)
                   - magDmg * (magPen + (1 - magPen) * 100 / (model.magDef + 100))
        );
        if (attackerID != -1)
        {
            model.LastHittersID.Push(attackerID);
        }
    }

    protected virtual void Awake()
    {
        manager = FindObjectOfType<DamageManager>();
    }

    protected virtual void Update()
    {
        if (model.health == 0 && model.LastHittersID.Count > 0)
        {
            Character killer = Character.GetCharacterByID(model.LastHittersID.Peek());
            killer.model.golds += GetGoldBounty();
            killer.model.exp += GetExpBounty();
            model.LastHittersID.Clear();
            Realtime.Destroy(gameObject);
        }
        
        model.RegenTimer += Time.deltaTime;
        while (model.RegenTimer >= 1)
        {
            model.health = Math.Min(model.health + model.healthRegen, model.maxHealth);
            --(model.RegenTimer);
        }
    }
}
