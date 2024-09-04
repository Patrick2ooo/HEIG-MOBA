using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class CravacheSevereEnflammee : Item
{
    public CravacheSevereEnflammee()
    {
        
        // Load the sprite in the constructor
        sprite = Resources.Load<Sprite>("Images/CravacheSevereEnflammee");     
        
        if (sprite == null)
        {
            Debug.LogError("Failed to load sprite for Cravache Severe Enflammee. Check the path and ensure the image is in the Resources folder.");
        }
    }

    public override int GetAttack() {
        return 75;
    }

    public override int GetHealth() {
        return 750;
    }

    public override float GetCritChance() {
        return 15;
    }

    public override float GetCritDamage() {
        return 0.05;
    }

    public override List<string> GetRecipe() {
        return new(){"Cravache Sévère", "Gemme de Feu"};
    }

    public override bool IsCrafted() {
        return true;
    }

    public override uint GetCost() {
        return 1100; // 550 + 420 = 970
    }

    public override uint GetSellingCost() {
        return 1656;
    }

    public override string GetName() {
        return "Cravache Sévère Enflammée";
    }
}
