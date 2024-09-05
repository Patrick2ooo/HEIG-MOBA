using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TowerModel : RealtimeComponent<NormcoreTimer>
{
    private Realtime _realtime;
    
    private bool timeSet = false;
    private float shootingInterval = 5f; // Time between each shot
	private TowerScript tower;
    private bool firstShot = true;
    private RealtimeView _realtimeView;
    
    // Start is called before the first frame update
    void Start()
    {
        _realtimeView = GetComponent<RealtimeView>();

        // Set the destroy conditions
        /*model.destroyWhenOwnerLeaves = false; // The tower should persist even if the owner leaves
        model.destroyWhenLastClientLeaves = false; // The tower should persist until the last client leaves
        */
        tower = GetComponentInChildren<TowerScript>();
    }

    public double time
    {
        get
        {
            if (model == null)
            {
                return 0.0;
            }
            else if (model.time == 0.0)
            {
                return 0.0;
            }
            else return realtime.roomTime - model.time;
        }
	}
    
	

    // Update is called once per frame
    void Update()
    {
        GameObject projectileShot = null;
        
        if (!_realtimeView.isOwnedRemotelySelf && !_realtimeView.isOwnedLocallyInHierarchy)
        {
            _realtimeView.RequestOwnership();
        }
        else
        {
            if (((time >= shootingInterval) || firstShot) && model.isOwnedLocallyInHierarchy)
            {
                if (!timeSet && time == 0 && realtime.roomTime != 0 && firstShot)
                {
                    timeSet = true;
                    model.time = realtime.roomTime;
                }

                Transform target = tower.SelectTarget();
                if (target != null)
                {
                    firstShot = false;
                    projectileShot = tower.Shoot(target);
                    model.time = realtime.roomTime; // Reset the timer after shooting
                }
                else
                {
                    firstShot = true;
                }
            }
            /*if (projectileShot == null)
            {
                model.ClearOwnership(true);
            }*/
        }
    }
}
