using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float moveSpeed = 10;
    private Vector3 moveDirection;
    private Transform target; // Reference to the target
    private bool isTracking = false; // Flag to check if the projectile should track the target

    void Update()
    {
        if (isTracking && (target != null))
        {
            // Track the target by moving towards it
            
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        }
        else if (moveDirection != Vector3.zero)
        {
            // Move in a fixed direction
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
        isTracking = false; // Ensure the projectile does not track a target
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        isTracking = true; // Enable tracking behavior
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Add the logic of what happens when the projectile hits something (like a player)
        // Use the layer system to know if it's hitting a player, wall, or minion
        // If it's a player, reduce the player's health; if it's a wall, destroy the projectile;
        // if it's a minion, reduce the minion's health

        // Example (assuming you have layers set up):
        // if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     // Reduce health
        // }

        // Destroy the projectile when it hits player 

        if (collision.gameObject.layer == LayerMask.NameToLayer("character"))
        {
            Destroy(gameObject);
        }
        
        
    }
}
