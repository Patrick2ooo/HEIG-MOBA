using System;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

public class DamageManager : RealtimeComponent<DamageManagerModel>
{
    public Character player;

    public void AddDamage(Entity target, float physDamage, float magDmg, float physPen, float magPen, float critChance, float critMult)
    {
        DamageModel instance = new DamageModel
        {
            attacker = player.GetPlayerID(),
            physDamage = physDamage,
            magDamage = magDmg,
            physPen = physPen,
            magPen = magPen,
            critChance = critChance,
            critMult = critMult,
            target = target is Character character ? character.GetPlayerID() : -1
        };
        
        model.damages.Add(instance);
    }

    private void Update()
    {
        foreach (var instance in model.damages)
        {
            if (instance.target == player.GetPlayerID())
            {
                Debug.Log("Health before = " + player.GetHealth());
                player.ReceiveDamage(instance.attacker, instance.physDamage, instance.magDamage, instance.physPen, instance.magPen, instance.critChance, instance.critMult);
                model.damages.Remove(instance);
                Debug.Log("Health after = " + player.GetHealth());
            }
        }
    }
}
