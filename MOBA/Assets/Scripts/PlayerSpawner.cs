using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine.Animations;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    public int status = 0;
    private Realtime _realtime;

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        GameObject playerObject = Realtime.Instantiate(prefabName: "PlayerComponents", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        PlayerScript player = playerObject.transform.GetChild(0).gameObject.GetComponent<PlayerScript>();
        player.mainCamera = _camera;
        /*ParentConstraint cameraConstraint = _camera.GetComponent<ParentConstraint>();
        ConstraintSource source = new ConstraintSource { sourceTransform = player.transform, weight = 1.0f };
        if (!cameraConstraint) status = 1;
        int constraintIndex = cameraConstraint.AddSource(source);
        cameraConstraint.SetTranslationOffset(constraintIndex, new Vector3(0, 15, -8));
        cameraConstraint.SetRotationOffset(constraintIndex, new Vector3(60, 0, 0));*/
        _camera.GetComponent<CameraScript>().target = player.transform;
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
