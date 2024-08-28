using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Normal.Realtime;

public class MinionScript : Entity
{
    public Vector3 destination;

    private NavMeshAgent _agent;
    
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
        _agent = this.GetComponent<NavMeshAgent>();
        _agent.SetDestination(destination);
        model.health = 10;
        radius = 0.4f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (model.health <= 0)
        {
            Realtime.Destroy(gameObject);
        }
    }
}
