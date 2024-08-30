using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : Entity
{
    public override int GetGoldBounty()
    {
        return 0;
    }

    public override int GetExpBounty()
    {
        return 0;
    }

    protected override void SetValues(Attributes model)
    {
        model.maxHealth = 5500;   
        model.health = 5500;
        model.healthRegen = 20;
    }
}
