using System;
using System.Collections;
using System.Linq;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Entity : RealtimeComponent<Attributes>{
    
    public NavMeshAgent agent;
    public DamageManager damageManager;
    public ExpGoldsManager expGoldsManager;
    protected Entity Target;
    protected float WindUpDuration, AttackDuration, RecoveryDuration;

    protected abstract int GetGoldBounty();
    protected abstract int GetExpBounty();
    protected abstract void SetValues(Attributes model);

    public static Entity GetEntityByID(string id)
    {
        return FindObjectsByType<Entity>(FindObjectsSortMode.None).FirstOrDefault(character => character.GetID() == id);
    }

    public void SetID(string id)
    {
        model.entityID = id;
    }
    
    public string GetID()
    {
        return model.entityID;
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

    public float GetRadius()
    {
        return model.radius;
    }
    
    public float GetAttack()
    {
        return model.attack;
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
        if (model.attackTime <= 0)
        {
            model.attackTime += AttackDuration;
            StartCoroutine(StartAttack(target));
        }
    }

    protected virtual IEnumerator StartAttack(Entity target)
    {
        yield return new WaitForSeconds(model.attackTime);
        if(!damageManager){
            damageManager = FindObjectOfType<DamageManager>();
        }
        damageManager.AddDamage(target, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
        model.recoveryTime = RecoveryDuration;
        model.attackTime = 0;
    }

    private void UpdateMoveSpeed(Attributes updated, float speed)
    {
        agent.speed = speed;
    }

    public void ReceiveDamage(string attackerID, float physDmg, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        if (model.health <= 0) return; // in case the entity receive damage after its death for whatever reason
        // dégâts reçus = dégâts de base * (pen + (1-pen) * 100 / (def + 100))
        float phys = physDmg * (physPen + (1 - physPen) * 100 / (model.physDef + 100));
        model.health = Math.Max(0, 
            model.health - (critChance >= Random.Range(0, 1) ? phys * (1.5f + critMult) : phys)
                   - magDmg * (magPen + (1 - magPen) * 100 / (model.magDef + 100))
        );
        if (GetEntityByID(attackerID) is Character)
        {
            model.LastHittersID.Push(attackerID);
        }
    }

    protected virtual void Update()
    {
        if (model.health <= 0)
        {
            if (model.LastHittersID.Count > 0)
            {
                Entity killer = GetEntityByID(model.LastHittersID.Peek());
                if (killer is Character)
                {
                    expGoldsManager.AddGain(killer, GetExpBounty(), GetGoldBounty());
                }
            }
            model.LastHittersID.Clear();
            KillSelf();
        }
        
        model.RegenTimer += Time.deltaTime;
        while (model.RegenTimer >= 1)
        {
            model.health = Math.Min(model.health + model.healthRegen, model.maxHealth);
            --(model.RegenTimer);
        }
    }

    protected virtual void KillSelf()
    {
        Realtime.Destroy(gameObject);
    }
}
