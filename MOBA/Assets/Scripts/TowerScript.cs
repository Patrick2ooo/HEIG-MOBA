using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TowerScript : Entity
{
    private static int id = 0;
    
    public Transform towerTop; // Assign the tower top transform in the Inspector
    private List<Transform> playersInRange = new List<Transform>(); // List to track players in the detection zone
    private List<Transform> minionsInRange = new List<Transform>(); // List to track minions in the detection zone
    private Transform currentTarget = null; // The current target (player or minion)

    
    private float shootTimer = 0f;
    private bool oneShot = true;
    private Realtime _realtime;

    void Start()
    {
        _realtime = GetComponent<Realtime>();
        // Check for player disconnections every few seconds
        InvokeRepeating("CheckForDisconnectedPlayers", 2.0f, 2.0f);
        // Initialize the child collider script to reference this TowerScript
        TowerColliderTrigger trigger = GetComponentInChildren<TowerColliderTrigger>();
        if (trigger != null)
        {
            trigger.Initialize(this); // Pass this TowerScript to the child
        }
    }
    
    protected override int GetGoldBounty()
    {
        return 0;
    }
    
    protected override int GetExpBounty()
    {
        return 0;
    }
    
    protected override void SetValues(Attributes attributes)
    {
        attributes.maxHealth = 5500;   
        attributes.health = 5500;
        attributes.healthRegen = 20;
        attributes.entityID = "t" + (++id);
        attributes.radius = 0.825f;
    }

    public Transform SelectTarget()
    {
        // If there's already a current target, maintain it until it leaves the range
        if (currentTarget != null)
        {
            return currentTarget;
        }

        // Priority: Minions over Players
        if (minionsInRange.Count > 0)
        {
            currentTarget = minionsInRange[0]; // Target the first minion in range
        }
        else if (playersInRange.Count > 0)
        {
            currentTarget = playersInRange[0]; // If no minions, target the first player in range
        }
        return currentTarget; // No target available
    }

    public GameObject Shoot(Transform target)
    {
        Vector3 direction = (target.position - towerTop.position).normalized;
        GameObject projectile = Realtime.Instantiate("Projectile", towerTop.position, Quaternion.LookRotation(direction), ownedByClient: true,  preventOwnershipTakeover: true, useInstance: _realtime);
        // Get the ProjectileScript component from the projectile
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        // Set the player as the target for the projectile to track
        if (projectileScript != null && target != null)
        {
            projectileScript.SetTarget(target);
        }
        return projectile;
    }

    public void OnTriggerEnterFromChild(Collider other)
    {
        //should we check the side of the player and minion there?
        if (other.CompareTag("Player"))
        {
            // Add the player to the list
            if (!playersInRange.Contains(other.transform) && other.GetComponent<Entity>().GetSide() != GetSide())
            {
                playersInRange.Add(other.transform);
            }

            // Update target if there's no current target
            if (currentTarget == null && other.GetComponent<Entity>().GetSide() != GetSide())
            {
                currentTarget = other.transform;
            }
        }
        else if (other.CompareTag("Minion"))
        {
            // Add the minion to the list
            if (!minionsInRange.Contains(other.transform) && other.GetComponent<Entity>().GetSide() != GetSide())
            {
                minionsInRange.Add(other.transform);
            }

            // Update target if there's no current target
            if (currentTarget == null && other.GetComponent<Entity>().GetSide() != GetSide())
            {
                currentTarget = other.transform;
            }
        }
    }

    public void OnTriggerExitFromChild(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the player from the list
            playersInRange.Remove(other.transform);

            // If the current target was this player, reset the current target
            if (currentTarget == other.transform)
            {
                currentTarget = null;
            }
        }
        else if (other.CompareTag("Minion"))
        {
            // Remove the minion from the list
            minionsInRange.Remove(other.transform);

            // If the current target was this minion, reset the current target
            if (currentTarget == other.transform)
            {
                currentTarget = null;
            }
        }
    }
    
    private void CheckForDisconnectedPlayers()
    {
        // Remove any null or invalid players from the list
        playersInRange.RemoveAll(player => player == null || !player.gameObject.activeInHierarchy);

        // Remove any null or invalid minions from the list
        minionsInRange.RemoveAll(minion => minion == null || !minion.gameObject.activeInHierarchy);

        // If the current target is no longer valid, reset it
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = null;
        }
    }

    protected override void KillSelf()
    {
        Realtime.Destroy(transform.parent.gameObject);
    }
}
