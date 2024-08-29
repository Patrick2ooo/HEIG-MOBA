using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Normal.Realtime;

public class MinionScript : Entity
{
    public Vector3 destination;
    private Queue<Entity> _targets = new Queue<Entity>();
    
    public override int GetGoldBounty()
    {
        return 1;
    }

    public override int GetExpBounty()
    {
        return 1;
    }

    // Start is called before the first frame update

    void Start()
    {
        agent.SetDestination(destination);
        model.health = 10;
        model.attackRange = 1;
        model.attack = 1;
        radius = 0.4f;
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
            if (Vector3.Distance(transform.position, Target.transform.position) - radius - Target.radius <=
                model.attackRange)
            {
                if (DealAutoDamage(Target))
                {
                    _targets.Dequeue();
                    while (_targets.Count > 0 && !_targets.First())
                    {
                        _targets.Dequeue();
                    }
                    if (_targets.Count > 0)
                    {
                        Target = _targets.First();
                    }
                    else
                    {
                        agent.destination = destination;
                    }
                }
            }
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
