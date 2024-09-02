using Normal.Realtime;
using UnityEngine;

public class MinionSpawner : RealtimeComponent<NormcoreTimer>
{
    public const double CycleLength = 60;

    public Vector3 leftSideSpawner, rightSideSpawner;

    private bool _timeSet;

    public double Time
    {
        get
        {
            if (model == null)
            {
                return 0.0;
            }
            else if (model.time == 0.0)
            {
                return 0.0;
            }
            else return realtime.roomTime - model.time;
        }
    }

    private void Awake()
    {
        GetComponent<Realtime>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_timeSet && Time == 0 && realtime.roomTime != 0)
        {
            _timeSet = true;
            model.time = realtime.roomTime;
        }
        
        if ((model.minionWaves + 1) * 10 < Time)
        {
            model.RequestOwnership(true);
            if (model.isOwnedLocallyInHierarchy && (model.minionWaves + 1) * 10 < Time)
            {
                model.minionWaves += 1;
                int minionsToSpawn = 1;
                if (Time % (2 * CycleLength) >= CycleLength) minionsToSpawn *= 2;
                for (int i = 0; i < minionsToSpawn; ++i)
                {
                    MinionScript minionLeft = Realtime.Instantiate("Minion", leftSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                    minionLeft.destination = rightSideSpawner;
                    minionLeft.SetSide(0);
                    minionLeft.name = "minionLeft " + i;
                    MinionScript minionRight = Realtime.Instantiate("Minion", rightSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                    minionRight.destination = leftSideSpawner;
                    minionRight.SetSide(1);
                    minionRight.name = "minionRight" + i;
                } 
            }
            model.ClearOwnership(true);
        }
    }
}
