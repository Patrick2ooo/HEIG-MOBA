using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Scripts;

public class ShopAction : MonoBehaviour
{

    public GameObject shopMenu;
    protected Item itemSelected;
    protected TMP_Text lblName;
    
    public void Show()
    {
        //SceneManager.LoadScene("Shop");
        
        
        // Enable the GameObject
        shopMenu.SetActive(true);
        lblName = shopMenu.transform.Find("Canvas").Find("Panel").Find("ItemDescriptionSection").Find("ItemDescriptionPanel").Find("Name").GetComponent<TMP_Text>();
        Debug.Log("GameObject activated!");
    }

    public void SelectGemmeDeFeu() {
        itemSelected = Item.GetItemByName("Gemme de Feu");
        //display info
        lblName.text = "Gemme de Feu";
    }

    protected void DisplayInfo() {

    }

    public void Buy() {
        gameObject.GetComponent<Character>().BuyItem(itemSelected.GetName());
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
