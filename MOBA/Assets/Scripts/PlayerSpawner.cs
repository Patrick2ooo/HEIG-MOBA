using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;
    private Realtime _realtime;
    public GameObject ui;
    public GameObject deathScreen;
    public Vector3 leftBase, rightBase;

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
        _realtime.didDisconnectFromRoom += Disconnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        ushort side = (ushort)(FindObjectsOfType<Character>().Length % 2);
        GameObject playerObject = Realtime.Instantiate(prefabName: "PlayerComponents", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        PlayerScript player = playerObject.transform.GetChild(0).gameObject.GetComponent<PlayerScript>();
        player.mainCamera = playerCamera;
        player.SetSide(side);
        FindObjectOfType<DamageManager>().player = player;
        player.SetPlayerID(realtime.clientID);
        player.deathScreen = deathScreen;
        player.playerBase = side == 0 ? leftBase : rightBase;
        playerCamera.GetComponent<CameraScript>().target = player.transform;
        player.InitInventory();
        Instantiate(ui);
        GameObject.FindWithTag("spellA").GetComponent<Button>().onClick.AddListener(player.SpellA);
        GameObject.FindWithTag("spellB").GetComponent<Button>().onClick.AddListener(player.SpellB);
        GameObject.FindWithTag("spellC").GetComponent<Button>().onClick.AddListener(player.SpellC);
        HealthBar.PlayerSide = player.GetSide();
    }

    private void Disconnect(Realtime realtime)
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length == 1) Realtime.Destroy(GameObject.FindWithTag("minionSpawner"));
    }
}
