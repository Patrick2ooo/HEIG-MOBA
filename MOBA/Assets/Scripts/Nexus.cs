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

    void Start()
    {
        model.maxHealth = 5500;   
        model.health = 5500;
        model.healthRegen = 20;
        setSide(1);
    }

    void Update() {
        base.Update();
        model.RegenTimer += Time.deltaTime;
        while (model.RegenTimer > 1) {
            model.health = model.health >= model.maxHealth ? model.health : model.health + model.healthRegen;
            --model.RegenTimer;
        }
    }
}
