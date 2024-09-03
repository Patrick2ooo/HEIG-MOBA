using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;


namespace Scripts
{

    [RealtimeModel]
    public partial class Item {

        protected Sprite sprite;

        public Item(bool forCompilErroWorkAround) {
            // Load an empty sprite in the constructor for exception managment
            sprite = Resources.Load<Sprite>("Images/white_pixel");     
            
            if (sprite == null)
            {
                Debug.LogError("Failed to load sprite for Item. Check the path and ensure the image is in the Resources folder.");
            }
        }
        
        public virtual int GetAttack(){
            return 0;
        }

        public virtual int GetHealth() {
            return 0;
        }

        public virtual float GetCritChance() {
            return 0;
        }
        
        public virtual List<string> GetRecipe() {
            return new List<string>();
        }

        public virtual bool IsCrafted() {
            return false;
        }

        public virtual bool IsActivable() {
            return false;
        }

        public virtual uint GetCost() {
            return 0;
        }

        public virtual uint GetSellingCost() {
            return (uint) (GetCost()*0.4);
        }

        public virtual string GetName() {
            return "Item";
        }


        public Sprite GetSprite() {
            return sprite;
        }

        public virtual string GetDescription() {
            StringBuilder sb = new StringBuilder();

            if(GetAttack() != 0) sb.AppendLine("Attack : " + GetAttack() + "\n");
            if(GetHealth() != 0) sb.AppendLine("Health : " + GetHealth() + "\n");
            if(GetCritChance() != 0) sb.AppendLine("Critical Chance : " + GetCritChance() + "\n");

            return sb.ToString();
        }

        public static Item GetItemByName(string itemName) {
            switch(itemName) {
                case "Cravache Sévère Enflammée":
                    return new CravacheSevereEnflammee();

                case "Cravache Sévère":
                    return new CravacheSevere();
                case "Gemme de Feu":
                    return new GemmeDeFeu();
                default:

                    return new Item(true);
            }
        }
    }
}