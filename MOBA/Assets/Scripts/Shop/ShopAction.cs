using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Normal.Realtime;
using TMPro;
using Scripts;

public class ShopAction : MonoBehaviour
{

    public static Character player;
    public Item itemSelected;
    public InventoryManagement inventoryManager;
    private bool isSelling;

    public GameObject shopMenu;
    
    public TMP_Text lblName;
    public Image image;
    public TMP_Text description;
    public TMP_Text cost;
    public TMP_Text buy;
    
    public void Show() {   
        // Enable the GameObject
        shopMenu = GameObject.FindWithTag("shopMenu");
        shopMenu.transform.Find("Canvas").gameObject.SetActive(true);

        //get refs
        lblName = GameObject.FindWithTag("selectedItemName").GetComponent<TMP_Text>();
        image = GameObject.FindWithTag("selectedItemLogo").GetComponent<Image>();
        description = GameObject.FindWithTag("selectedItemDescription").GetComponent<TMP_Text>();
        cost = GameObject.FindWithTag("cost").GetComponent<TMP_Text>();
        buy = GameObject.FindWithTag("buy").GetComponent<TMP_Text>();

        //Reset to default values
        itemSelected = new Item(true);
        SelectItem();
    }

    public void SelectGemmeDeFeu() {
        itemSelected = Item.GetItemByName("Gemme de Feu");
        SelectItem();
    }

    public void SelectCravacheSevere() {
        itemSelected = Item.GetItemByName("Cravache Sévère");
        SelectItem();
    }

    public void SelectCravacheSevereEnflammee() {
        itemSelected = Item.GetItemByName("Cravache Sévère Enflammée");
        SelectItem();
    }

    private void SelectItem() {
        isSelling = false;
        DisplayInfo();
    }

    protected void DisplayInfo() {
        lblName.text = itemSelected.GetName() == "Item" ? "" : itemSelected.GetName();
        image.enabled = itemSelected.GetName() != "Item";
        image.sprite = itemSelected.GetSprite();
        description.text = itemSelected.GetDescription();
        cost.text = new StringBuilder("Cost: ").AppendLine(itemSelected.GetName() == "Item" ? "-" : itemSelected.GetCost().ToString()).ToString();
        buy.text = isSelling ? "Sell" : "Buy";
    }

    public void SelectSlot0() {SelectSlot(0);}
    public void SelectSlot1() {SelectSlot(1);}
    public void SelectSlot2() {SelectSlot(2);}
    public void SelectSlot3() {SelectSlot(3);}
    public void SelectSlot4() {SelectSlot(4);}
    public void SelectSlot5() {SelectSlot(5);}
    private void SelectSlot(uint slotId) {
        isSelling = true;
        itemSelected = player.GetItem(slotId);
        DisplayInfo();
    }

    public void Buy() {
        player.BuyItem(itemSelected.GetName());
        inventoryManager = GetComponent<InventoryManagement>();
        Debug.Log("attack : " + player.GetAttack());
        inventoryManager.ItemChange();
        Debug.Log("2attack : " + player.GetAttack());

    }
    
    public void Close() {
        shopMenu.transform.Find("Canvas").gameObject.SetActive(false);
    }
}
