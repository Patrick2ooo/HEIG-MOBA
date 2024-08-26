using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject minionPrefab;

    private float _spawnerTimer = 0;

    private Realtime _realtime;
    
    public Vector3 leftSideSpawner, rightSideSpawner;

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
