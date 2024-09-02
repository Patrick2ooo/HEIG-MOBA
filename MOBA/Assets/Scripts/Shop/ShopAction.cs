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
    protected GameObject shopMenu;
    protected Item itemSelected;
    protected TMP_Text lblName;
    protected Image image;
    protected TMP_Text description;
    protected TMP_Text cost;
    
    public void Show() {   
        // Enable the GameObject
        shopMenu = GameObject.FindWithTag("shopMenu");
        shopMenu.transform.Find("Canvas").gameObject.SetActive(true);

        //get refs
        lblName = shopMenu.transform.FindWithTag("selectedItemName").GetComponent<TMP_Text>();
        image = shopMenu.transform.FindWithTag("selectedItemLogo").GetComponent<Image>();
        description = shopMenu.transform.FindWithTag("selectedItemDescription").GetComponent<TMP_Text>();
        cost = shopMenu.transform.FindWithTag("Cost").GetComponent<TMP_Text>();

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

    protected void DisplayInfo() {
        //display info
        lblName.text = itemSelected.GetName();
        image.enabled = true;
        //image.sprite = itemSelected.GetImagePath();
        description.text = itemSelected.GetDescription();
        cost.text = "Cost: " + itemSelected.GetCost();
    }

    public void Buy() {
        player.BuyItem(itemSelected.GetName());
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
