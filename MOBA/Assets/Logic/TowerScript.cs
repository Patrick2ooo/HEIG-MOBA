using System.Collections;
using UnityEngine;
using Normal.Realtime;

public class TowerScript : Entity
{
    public GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    public Transform towerTop; // Assign the tower top transform in the Inspector
    public float shootingInterval = 5f; // Time between each shot

    private bool canShoot = true;
    private Transform player = null;

    private Realtime _realtime;

    void Start()
    {
        _realtime = GetComponent<Realtime>();
    }
    public override int GetGoldBounty()
    {
        throw new System.NotImplementedException();
    }

    public override int GetExpBounty()
    {
        throw new System.NotImplementedException();
    }
    
    void Update()
    {
        if (canShoot && player != null)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {

        canShoot = false; // Prevent shooting until the interval passes
        
        Vector3 direction = (player.position - towerTop.position).normalized;
        
        GameObject projectile = Realtime.Instantiate("Projectile", towerTop.position, Quaternion.LookRotation(direction), preventOwnershipTakeover: true, useInstance: _realtime);

        // Get the ProjectileScript component from the projectile
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        // Set the player as the target for the projectile to track
        if (projectileScript != null && player != null)
        {
            projectileScript.SetTarget(player);
        }

        // Wait for the shooting interval
        yield return new WaitForSeconds(shootingInterval);

        canShoot = true; // Allow shooting again after the interval
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has a tag "Player"
        {
            player = other.transform; // Set the player transform when they enter the zone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null; // Clear the player transform when they leave the zone
        }
    }
}
