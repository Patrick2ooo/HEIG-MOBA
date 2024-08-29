using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{

    public class Item {

        protected string name;
        protected int attack;
        protected int health;

        public int GetAttack() {
            return attack;
        }

        public int GetHealth() {
            return health;
        }
        
        public bool IsActivable() {
            return false;
        }

        public string GetName() {
            return name;
        }

        public static Item GetItemByName(string itemName) {
            switch(itemName) {
                case "Cravache Sévère":
                    return new CravacheSevere();
                default:
                    return null;
            }
        }
    }
}