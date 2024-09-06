using UnityEngine;
using Normal.Realtime;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    public Camera playerCamera;
    private Realtime _realtime;
    public GameObject ui;
    public GameObject deathScreen;
    public static Vector3 LeftBase = new(-51, 0, 0), RightBase = new(51, 0, 0);
    public DamageManager damageManager;
    public ExpGoldsManager expGoldsManager;

    private void Awake()
    {
        _realtime = GetComponent<Realtime>();
        _realtime.didConnectToRoom += DidConnect;
        _realtime.didDisconnectFromRoom += Disconnect;
    }
    
    private void DidConnect(Realtime realtime)
    {
        ushort side = (ushort)(FindObjectsOfType<Character>().Length % 2);
        GameObject playerObject = Realtime.Instantiate("PlayerComponents", ownedByClient: true, preventOwnershipTakeover: true, useInstance: realtime);
        PlayerScript player = playerObject.GetComponentInChildren<PlayerScript>();
        player.transform.position = side == 0 ? LeftBase : RightBase;
        player.mainCamera = playerCamera;
        player.SetSide(side);
        damageManager.player = player;
        expGoldsManager.player = player;
        player.damageManager = damageManager;
        player.expGoldsManager = expGoldsManager;
        player.SetID("0" + realtime.clientID);
        player.deathScreen = deathScreen;
        player.playerBase = side == 0 ? LeftBase : RightBase;
        playerCamera.GetComponent<CameraScript>().target = player.transform;
        player.InitInventory();
        ui.SetActive(true);
        ShopAction shopAction = GameObject.FindWithTag("shopMenu").GetComponent<ShopAction>();
        GameObject.FindWithTag("shopButton").GetComponent<Button>().onClick.AddListener(shopAction.Show);
        ShopAction.player = player;
        InventoryManagement.myCharacter = player;
        GameObject.FindWithTag("spellA").GetComponent<Button>().onClick.AddListener(player.SpellA);
        GameObject.FindWithTag("spellB").GetComponent<Button>().onClick.AddListener(player.SpellB);
        GameObject.FindWithTag("spellC").GetComponent<Button>().onClick.AddListener(player.SpellC);
        HealthBar.PlayerSide = player.GetSide();
        playerCamera.gameObject.SetActive(true);
    }

    private void Disconnect(Realtime realtime)
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length == 1) Realtime.Destroy(GameObject.FindWithTag("minionSpawner"));
    }
}
