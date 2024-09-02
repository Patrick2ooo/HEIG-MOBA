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

    public Character player;
    protected GameObject shopMenu;
    protected Item itemSelected;
    protected TMP_Text lblName;
    protected Image image;
    protected TMP_Text description;
    protected TMP_Text cost;
    
    public void Show()
    {
        //SceneManager.LoadScene("Shop");
        
        
        // Enable the GameObject
        Debug.Log("Getting me...");
        //Transform me = player.transform;
        Debug.Log("Getting shop...   player:" + player);
        //Debug.Log("Getting shop...   me:" + me);
        //Transform shop = me.Find("ShopMenu");
        //Debug.Log("Getting shop GO...  shop:" + shop);
        shopMenu = gameObject;
        Debug.Log("Activate shop...");
        shopMenu.SetActive(true);

        /*//get refs
        Debug.Log("Getting refs...");
        lblName = shopMenu.transform.Find("Canvas").Find("Panel").Find("ItemDescriptionSection").Find("ItemDescriptionPanel").Find("Name").GetComponent<TMP_Text>();
        image = shopMenu.transform.Find("Canvas").Find("Panel").Find("ItemDescriptionSection").Find("Item").Find("Item1Logo").GetComponent<Image>();
        description = shopMenu.transform.Find("Canvas").Find("Panel").Find("ItemDescriptionSection").Find("ItemDescriptionPanel").Find("Panel").Find("Text (TMP)").GetComponent<TMP_Text>();
        cost = shopMenu.transform.Find("Canvas").Find("Panel").Find("PurchasePanelSection").Find("PurchasePanel").Find("PurchaseSections").Find("CostSection").Find("GameObject").GetComponent<TMP_Text>();

        //Reset to default values
        Debug.Log("Setting defaults values...");
        lblName.text = "";
        image.enabled = false;
        description.text = "";
        cost.text = "Cost: -";
        Debug.Log("Done");*/
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
    
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopMenu != null)
            {
                // Deactivate the GameObject
                shopMenu.SetActive(false);
                Debug.Log("GameObject deactivated!");
            }
        }
    }
}
