using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Normal.Realtime;
using TMPro;
using Scripts;

public class ShopAction : MonoBehaviour
{

    public static Character player;
    public GameObject shopMenu;
    public Item itemSelected;
    public TMP_Text lblName;
    public Image image;
    public TMP_Text description;
    public TMP_Text cost;
    
    public void Show() {   
        // Enable the GameObject
        shopMenu = GameObject.FindWithTag("shopMenu");
        shopMenu.transform.Find("Canvas").gameObject.SetActive(true);

        //get refs
        lblName = GameObject.FindWithTag("selectedItemName").GetComponent<TMP_Text>();
        image = GameObject.FindWithTag("selectedItemLogo").GetComponent<Image>();
        description = GameObject.FindWithTag("selectedItemDescription").GetComponent<TMP_Text>();
        cost = GameObject.FindWithTag("cost").GetComponent<TMP_Text>();

        //Reset to default values
        lblName.text = "";
        image.enabled = false;
        description.text = "";
        cost.text = "Cost: -";
    }

    public void SelectGemmeDeFeu() {
        itemSelected = Item.GetItemByName("Gemme de Feu");
        DisplayInfo();
    }

    public void SelectCravacheSevere() {
        itemSelected = Item.GetItemByName("Cravache Sévère");
        DisplayInfo();
    }

    public void SelectCravacheSevereEnflammee() {
        itemSelected = Item.GetItemByName("Cravache Sévère Enflammée");
        DisplayInfo();
    }

    protected void DisplayInfo() {
        //display info
        lblName.text = itemSelected.GetName();
        image.enabled = true;
        image.sprite = itemSelected.GetSprite();
        description.text = itemSelected.GetDescription();
        cost.text = "Cost: " + itemSelected.GetCost();
    }


    public InventoryManagement inventoryManager;
    public void Buy() {
        player.BuyItem(itemSelected.GetName());
        inventoryManager = GetComponent<InventoryManagement>();
        Debug.Log("attack : " + player.GetAttack());
        inventoryManager.Delay();
        Debug.Log("2attack : " + player.GetAttack());
    }
    
    public void Close() {
        //if (shopMenu != null) {
            // Deactivate the GameObject
            Debug.Log("Closing");
            shopMenu.transform.Find("Canvas").gameObject.SetActive(false);
            Debug.Log("Closed");
        //}
    }
}
