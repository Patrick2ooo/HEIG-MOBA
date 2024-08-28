using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Realtime _realtime;
    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        if (GameObject.FindWithTag("minionSpawner") == null)
        {
            Realtime.Instantiate(prefabName: "MinionSpawner", preventOwnershipTakeover: true, useInstance: realtime);
        } 
    }
}
