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
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        while (_targets.Count > 0 && !_targets.First())
        {
            _targets.Dequeue();
        }
        if (model.health <= 0)
        {
            Realtime.Destroy(gameObject);
        }

        if (Target)
        {
            agent.destination = Target.transform.position;
            if (Vector3.Distance(transform.position, Target.transform.position) - model.radius - Target.GetRadius()
                <= model.attackRange)
            {
                DealAutoDamage(Target);
            }
        } 
        else if (_targets.Count > 0)
        {
            Target = _targets.First();
        }
        else
        {
            agent.destination = destination;
        }
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
