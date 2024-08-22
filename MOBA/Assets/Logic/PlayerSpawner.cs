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
        GameObject character = Realtime.Instantiate(prefabName: "Player", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        PlayerScript player = character.GetComponent<PlayerScript>();
        Camera cam = new Camera();
        CameraScript camLogic = cam.AddComponent<CameraScript>();
        camLogic.target = character.transform;
        player.mainCamera = cam;
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
