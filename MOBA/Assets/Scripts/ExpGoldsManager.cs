using System;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

public class ExpGoldsManager : RealtimeComponent<ExpGoldsManagerModel>
{
    public Character player;

    public void AddGain(Entity target, int exp, int golds)
    {
        ExpGoldsModel gain = new ExpGoldsModel()
        {
            target = target.GetID(),
            exp = exp,
            golds = golds
        };
        model.gains.Add(gain);
    }

    private void Update()
    {
        foreach (var gain in model.gains)
        {
            if (gain.target == player.GetPlayerID())
            {
                player.AddExpGolds(gain.exp, gain.golds);
                model.gains.Remove(gain);
            }
        }
    }
}