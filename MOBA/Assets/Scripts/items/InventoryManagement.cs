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
    
    public Character myCharacter;
    
    // This returns the GameObject named Hand.
    public GameObject[] Icons = new GameObject[6];
    
    // Start is called before the first frame update
    void Start()
    {
        maCravache = new CravacheSevere();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ItemChange()
    {
        Item[] items = myCharacter.GetInventory();

        for (int i = 0; i < 6; ++i)
        {
            Icons[i] = GameObject.Find("ItemIcon" + i);
        }
        
        Sprite[] sprites = new Sprite[6];
        
        for (int i = 0; i < 6; ++i)
        {
            if (items[i].GetName() != "item")
            {
                Icons[i].SetActive(true);
                Icons[i].GetComponent<Image>().sprite = items[i].GetSprite();   
            }
            
        }
        //newImage = maCravache.sprite;
        //Debug.Log("attack : " + maCravache.GetAttack());
        //oldImage.sprite = newImage;
    }
}
