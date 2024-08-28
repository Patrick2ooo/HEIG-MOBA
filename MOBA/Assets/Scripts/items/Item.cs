using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts
{

    public class Item : MonoBehaviour {
        
        void start() {
            
        }

        public int GetAttack() {
            return 0;
        }

        public int GetHealth() {
            return 0;
        }
        
        public bool IsActivable() {
            return false;
        }

        public string GetName() {
            return "Item";
        }
        
        void Update() {
            
        }
    }
}