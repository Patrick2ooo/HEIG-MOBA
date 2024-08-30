using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowShop : MonoBehaviour
{

    public GameObject shopMenu;
    
    public void Show()
    {
        //SceneManager.LoadScene("Shop");
        
        
        // Enable the GameObject
        shopMenu.SetActive(true);
        Debug.Log("GameObject activated!");
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
