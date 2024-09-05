using Normal.Realtime;
using UnityEngine;

public class MinionSpawner : RealtimeComponent<NormcoreTimer>
{
    public const double CycleLength = 60;

    private Vector3 leftSideSpawner = new(-51, 0, 0), rightSideSpawner = new(51, 0, 0);

    public DamageManager damageManager;
    public ExpGoldsManager expGoldsManager;

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
                    minionLeft.SetID("1" + model.minionWaves + i + "0");
                    minionLeft.damageManager = damageManager;
                    minionLeft.expGoldsManager = expGoldsManager;
                    MinionScript minionRight = Realtime.Instantiate("Minion", rightSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                    minionRight.destination = leftSideSpawner;
                    minionRight.SetSide(1);
                    minionRight.SetID("1" + model.minionWaves + i + "1");
                    minionRight.damageManager = damageManager;
                    minionRight.expGoldsManager = expGoldsManager;
                } 
            }
            model.ClearOwnership(true);
        }
    }
}
