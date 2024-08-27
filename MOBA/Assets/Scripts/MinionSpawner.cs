using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Normal.Realtime;
using UnityEngine;

public class MinionSpawner : RealtimeComponent<NormcoreTimer>
{
    public GameObject minionPrefab;

    private float _spawnerTimer = 0;

    private Realtime _realtime;
    
    public Vector3 leftSideSpawner, rightSideSpawner;

    public float time
    {
        get
        {
            if (model == null)
            {
                return 0.0f;
            }
            if (model.time == 0.0f)
            {
                return 0.0f;
            }
            return (float) realtime.roomTime - model.time;
        }
    }

    protected override void OnRealtimeModelReplaced(NormcoreTimer previousModel, NormcoreTimer currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);
    }

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        Debug.Log(time);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time == 0 && realtime.roomTime != 0)
        {
            Debug.Log("changed");
            model.time = (float) realtime.roomTime;
        }
        // Debug.Log(realtime.roomTime);
        _spawnerTimer += Time.deltaTime;
        if (_spawnerTimer > 10)
        {
            _spawnerTimer -= 10;
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
    }
}
