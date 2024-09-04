using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;

public class MinionScript : Entity
{
    public Vector3 destination;
    private readonly Queue<Entity> _targets = new();

    protected override int GetGoldBounty()
    {
        return 30;
    }

    protected override int GetExpBounty()
    {
        return 5;
    }

    protected override void SetValues(Attributes attributes)
    {
        agent.SetDestination(destination);
        attributes.maxHealth = 10;
        attributes.health = 10;
        attributes.attackRange = 1;
        attributes.attack = 1;
        attributes.radius = 0.4f;
        WindUpDuration = 0.4f;
        AttackDuration = 0.2f;
        RecoveryDuration = 0.4f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        while (_targets.Count > 0 && !_targets.First())
        {
            _targets.Dequeue();
        }

        if (Target && model.windUpTime + model.attackTime + model.recoveryTime <= 0)
        {
            agent.destination = Target.transform.position;
            if (Vector3.Distance(transform.position, Target.transform.position) - model.radius - Target.GetRadius()
                <= model.attackRange)
            {
                model.windUpTime = WindUpDuration;
                agent.ResetPath();
            }
        } 
        else if (_targets.Count > 0)
        {
            Target = _targets.First();
        }
        else if(model.attackTime + model.recoveryTime < 0)
        {
            agent.destination = destination;
        }

        if (model.windUpTime > 0)
        {
            model.windUpTime -= Time.deltaTime;
            if (model.windUpTime <= 0) model.attackTime = AttackDuration + model.windUpTime;
        }

        if (model.attackTime > 0)
        {
            model.attackTime -= Time.deltaTime;
            if (model.attackTime <= 0)
            {
                model.recoveryTime = RecoveryDuration + model.attackTime;
                Debug.Log("deal");
                DealAutoDamage(Target);
            }
        }

        if (model.recoveryTime > 0) model.recoveryTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.transform.parent.GetComponent<Entity>();
        if(e.GetSide() != model.side)
        {
            _targets.Enqueue(e);
            if (Target == null)
            {
                Target = e;
            }
        }
    }
}
