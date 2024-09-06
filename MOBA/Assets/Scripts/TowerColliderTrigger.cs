using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColliderTrigger : MonoBehaviour
{
    // Reference to the parent TowerScript
    private TowerScript towerScript;

    // Initialize the script with a reference to the parent TowerScript
    public void Initialize(TowerScript parentScript)
    {
        towerScript = parentScript;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Forward the trigger event to the parent TowerScript
        towerScript.OnTriggerEnterFromChild(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Forward the trigger exit event to the parent TowerScript
        towerScript.OnTriggerExitFromChild(other);
    }
}
