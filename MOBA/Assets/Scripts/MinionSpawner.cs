using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using Normal.Realtime;
using UnityEngine;

public class MinionSpawner : RealtimeComponent<NormcoreTimer>
{
    private Realtime _realtime;
    
    public Vector3 leftSideSpawner, rightSideSpawner;

    private bool timeSet = false;

    public double time
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
        _realtime = GetComponent<Realtime>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeSet && time == 0 && realtime.roomTime != 0)
        {
            timeSet = true;
            model.time = realtime.roomTime;
        }
        
        if ((model.minionWaves + 1) * 10 < time)
        {
            model.RequestOwnership(true);
            if (model.isOwnedLocallyInHierarchy && (model.minionWaves + 1) * 10 < time)
            {
                model.minionWaves += 1;
                for (int i = 0; i < 5; ++i)
                {
                    MinionScript minionLeft = Realtime.Instantiate("Minion", leftSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                    minionLeft.destination = rightSideSpawner;
                    minionLeft.setSide(0);
                    MinionScript minionRight = Realtime.Instantiate("Minion", rightSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                    minionRight.destination = leftSideSpawner;
                    minionRight.setSide(1);
                } 
            }
            model.ClearOwnership(true);
        }
    }
}
