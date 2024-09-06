using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{

    CravacheSevere maCravache;
    public Image oldImage;
    public Sprite newImage;
    
    public static Character myCharacter;
    
    
    public GameObject[] InventoryIcons = new GameObject[6];
    
    
    public GameObject[] StoreIcons = new GameObject[6];
    
    // Start is called before the first frame update
    void Start()
    {
        maCravache = new CravacheSevere();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delay()
    {
        float delay = 0.1f;
        Invoke(nameof(ItemChange), delay);
    }
    
    // Updates inventory Icons
    public void ItemChange()
    {
        Item[] items = myCharacter.GetInventory();
        
        Sprite[] sprites = new Sprite[6];
        
        for (int i = 0; i < 6; ++i)
        {

            if (items[i].GetName() != "Item")
            {
                Debug.Log(items[i].GetName());
                InventoryIcons[i].SetActive(true);
                InventoryIcons[i].GetComponent<Image>().sprite = items[i].GetSprite();   
                
                StoreIcons[i].SetActive(true);
                StoreIcons[i].GetComponent<Image>().sprite = items[i].GetSprite();   
            }
            else
            {
                InventoryIcons[i].SetActive(false);
                StoreIcons[i].SetActive(false);
            }
            
        }
        
    }

    public void GetCharacter()
    {
        Debug.Log(myCharacter);
    }
}
