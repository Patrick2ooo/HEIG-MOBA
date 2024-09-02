using System;
using Normal.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MinionSpawner _spawner;
    private Realtime _realtime;
    public Light globalLight;
    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        if (GameObject.FindWithTag("minionSpawner") == null)
        {
            _spawner = Realtime.Instantiate(prefabName: "MinionSpawner", preventOwnershipTakeover: true, useInstance: realtime).GetComponent<MinionSpawner>();
        } 
    }

    private void Update()
    {
        if (_spawner)
        {
            globalLight.intensity = (_spawner.Time % (2 * MinionSpawner.CycleLength) >= MinionSpawner.CycleLength) ? 0.5f : 1;
        }
    }
}
