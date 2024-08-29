using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    private Realtime _realtime;
    public GameObject UI;

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
        _camera.GetComponent<CameraScript>().target = player.transform;
        Instantiate(UI);
        if(GameObject.FindWithTag("spellA") != null) Debug.Log("fine");
        GameObject.FindWithTag("spellA").GetComponent<Button>().onClick.AddListener(player.SpellA);
        GameObject.FindWithTag("spellB").GetComponent<Button>().onClick.AddListener(player.SpellB);
        GameObject.FindWithTag("spellC").GetComponent<Button>().onClick.AddListener(player.SpellC);
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
