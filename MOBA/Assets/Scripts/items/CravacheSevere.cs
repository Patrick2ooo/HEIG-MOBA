using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class CravacheSevere : Item
{
    
    public CravacheSevere()
    {
        
        // Load the sprite in the constructor
        sprite = Resources.Load<Sprite>("Images/Cravache Severe");     
        
        if (sprite == null)
        {
            Debug.LogError("Failed to load sprite for Cravache Severe. Check the path and ensure the image is in the Resources folder.");
        }
    }
    
    public override int GetAttack() {
        return 50;
    }

    public override uint GetCost() {
        return 420;
    }

    public override string GetName() {
        return "Cravache Sévère";
    }
}
