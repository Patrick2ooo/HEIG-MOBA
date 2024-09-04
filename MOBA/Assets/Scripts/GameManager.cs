using System;
using Normal.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MinionSpawner _spawner;
    private Realtime _realtime;
    public Light globalLight;
    public DamageManager damageManager;
    public ExpGoldsManager expGoldsManager;
    private static Vector3 Offset = new(10, 0, 0);
    private Vector3 _leftBase, _rightBase;
    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        if (GameObject.FindWithTag("minionSpawner") == null)
        {
            _leftBase = PlayerSpawner.LeftBase;
            _rightBase = PlayerSpawner.RightBase;
            _spawner = Realtime.Instantiate(prefabName: "MinionSpawner", preventOwnershipTakeover: true, useInstance: realtime).GetComponent<MinionSpawner>();
            _spawner.damageManager = damageManager;
            _spawner.expGoldsManager = expGoldsManager;
            for (int i = 1; i < 4; ++i)
            {
                Realtime.Instantiate("TowerModel", _leftBase + i * Offset, Quaternion.identity, preventOwnershipTakeover: false, useInstance: realtime).GetComponentInChildren<TowerScript>().SetSide(0);
                Realtime.Instantiate("TowerModel", _rightBase - i * Offset, Quaternion.identity, preventOwnershipTakeover: false, useInstance: realtime).GetComponentInChildren<TowerScript>().SetSide(1);
            }
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
