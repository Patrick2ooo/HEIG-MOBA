using Normal.Realtime;

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
            target = target.GetID()
        };
        
        model.damages.Add(instance);
    }

    private void Update()
    {
        foreach (var instance in model.damages)
        {
            if (instance.target == player.GetPlayerID())
            {
                player.ReceiveDamage(instance.attacker, instance.physDamage, instance.magDamage, instance.physPen, instance.magPen, instance.critChance, instance.critMult);
                model.damages.Remove(instance);
            }else if (!instance.target.StartsWith("0"))
            {
                Entity e = Entity.GetEntityByID(instance.target);
                if (e)
                {
                    if (!e.isOwnedRemotelySelf && !e.isOwnedLocallyInHierarchy)
                    {
                        e.RequestOwnership();
                    }
                    e.ReceiveDamage(instance.attacker, instance.physDamage, instance.magDamage, instance.physPen, instance.magPen, instance.critChance, instance.critMult);
                }
                model.damages.Remove(instance);
            }
        }
    }
}
