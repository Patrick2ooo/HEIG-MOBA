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
        
        public virtual bool IsActivable() {
            return false;
        }

        public virtual uint GetCost() {
            return 0;
        }

        public virtual uint GetSellingCost() {
            return GetCost()*0.4;
        }

        public virtual string GetName() {
            return "Item";
        }

        public static Item GetItemByName(string itemName) {
            switch(itemName) {
                case "Cravache Sévère":
                    return new CravacheSevere();
                default:
                    return new Item();
            }
        }
    }
}