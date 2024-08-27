using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : Entity
{
    protected float HealthRegen = 20, RegTimer;
    public override int GetGoldBounty()
    {
        return 0;
    }

    public override int GetExpBounty()
    {
        return 0;
    }

    void Start()
    {
        MaxHealth = 5500;   
        Health = 5500;
    }

    void Update() {
        base.Update();
        RegTimer += Time.deltaTime;
        while (RegTimer > 1) {
            Health = Health >= MaxHealth ? Health : Health + HealthRegen;
            --RegTimer;
        }
    }
}
