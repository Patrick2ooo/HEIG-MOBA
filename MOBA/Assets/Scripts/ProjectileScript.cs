using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ProjectileScript : Entity
{
    public float moveSpeed = 10;
    private Vector3 moveDirection;
    private Transform target; // Reference to the target
    private bool isTracking = false; // Flag to check if the projectile should track the target
    
    void Start() {
        // Subscribe to the event when isDestroyed changes
        model.isDestroyedDidChange += HandleIsDestroyedDidChange;
    }

    void OnDestroy() {
        // Unsubscribe from the event to avoid memory leaks
        model.isDestroyedDidChange -= HandleIsDestroyedDidChange;
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
        //à voir
        attributes.attackRange = 1;
        attributes.attack = 100;
        attributes.radius = 0.4f;
    }

    protected override void DealAutoDamage(Entity target)
    {
        if (model.attackTime <= 0)
        {
            model.attackTime += AttackDuration;
            if(!damageManager)
            {
            damageManager = FindObjectOfType<DamageManager>();
            }
            damageManager.AddDamage(target, model.attack, 0, model.physPen, model.magPen, model.critChance, model.critMult);
            model.recoveryTime = RecoveryDuration;
            model.attackTime = 0;
        }
    }


    void Update()
    {
        if (model.isDestroyed) {
            // If already marked as destroyed, exit early
            return;
        }
        
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
    
    private void HandleIsDestroyedDidChange(Attributes model, bool isDestroyed) {
        if (isDestroyed) {
            // Destroy the projectile locally
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter(Collision collision)

    {   
        //when hitting the target, get his model and reduce his health by the attack value of the projectile
        if (collision.gameObject.CompareTag("Minion")) {
            DealAutoDamage(collision.transform.GetComponent<Entity>());
            model.isDestroyed = true;
        }             
        //if collision with an object that have the tag player, destroy the projectile
        if (collision.gameObject.CompareTag("Player"))
        {
            DealAutoDamage(collision.transform.GetComponent<Entity>());
            model.isDestroyed = true; 
        }           
                    
    }
}
