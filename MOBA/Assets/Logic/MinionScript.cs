using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        Health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
