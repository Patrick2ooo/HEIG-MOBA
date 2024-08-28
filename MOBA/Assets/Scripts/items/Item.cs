using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{

    public class Item : MonoBehaviour {

        protected string name;
        protected int attack;
        protected int health;
        
        void start() {
            
        }

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
        
        void Update() {
            
        }
    }
}