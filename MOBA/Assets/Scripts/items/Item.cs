using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{

    [RealtimeModel]
    public partial class Item {

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

        public static Item GetItemByName(string itemName) {
            switch(itemName) {
                case "Cravache Sévère":
                    return new CravacheSevere();
                case "Gemme de Feu":
                    return new GemmeDeFeu();
                default:
                    return new Item();
            }
        }
    }
}