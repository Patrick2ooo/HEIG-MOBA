using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Unity.VisualScripting;

public class PlayerSpawner : MonoBehaviour
{

    private Realtime _realtime;

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
    }

    private void DidConnect(Realtime realtime)
    {
        Debug.Log("connected");
        Realtime.Instantiate(prefabName: "Player", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
