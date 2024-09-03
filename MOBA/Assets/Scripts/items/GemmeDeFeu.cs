using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class GemmeDeFeu : Item
{
    public GemmeDeFeu()
    {
        
        // Load the sprite in the constructor
        sprite = Resources.Load<Sprite>("Images/Gemme De Feu");     
        
        if (sprite == null)
        {
            Debug.LogError("Failed to load sprite for Gemme De Feu. Check the path and ensure the image is in the Resources folder.");
        }
    }

    public override int GetHealth() {
        return 500;
    }

    public override uint GetCost() {
        return 550;
    }

    public override string GetName() {
        return "Gemme de Feu";
    }
}
