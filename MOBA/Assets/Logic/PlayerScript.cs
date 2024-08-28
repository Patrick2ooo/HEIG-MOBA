using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Normal.Realtime;

public class PlayerScript : Character
{
    public Camera mainCamera;
    private const int MapLayer = 3, CharactersLayer = 6;
    public GameObject icon;
    public NavMeshAgent nav;
    private static readonly Vector3 Offset = new(0, 0.1f, 0);
    private RealtimeView _view;
    private Realtime _realtime;

    public GameObject projectile; 

    private void Awake()
    {
        _view = GetComponent<RealtimeView>();
    }

    public override int GetGoldBounty()
    {
        throw new System.NotImplementedException();
    }

    public override int GetExpBounty()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        AttackRange = 1.5f;
        Attack = 2;
        AttackPerLevel = 1;
        _realtime = GetComponent<Realtime>();
        if (_view.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (_view.isOwnedLocallyInHierarchy)
        {
            base.Update();
            if (Input.GetMouseButton(0))
            {
                // Create a layer mask that ignores the "Tower" layer
                int layerMask = ~(1 << LayerMask.NameToLayer("Tower"));

                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    switch (hit.collider.gameObject.layer)
                    {
                        case MapLayer:
                            if(icon) Instantiate(icon, hit.point + Offset, Quaternion.identity);
                            nav.SetDestination(hit.point);
                            Target = null;
                            break;
                        case CharactersLayer:
                            Target = hit.collider.gameObject.GetComponent<Entity>();
                            Vector3 pos = Target.transform.position;
                            if (Target.side == side)
                            {
                                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, ~(1 << CharactersLayer));
                                pos = hit.point;
                                Target = null;
                            }
                            nav.SetDestination(pos);
                            break;
                    }
                }
            }
            if (Target)
            {
                if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange)
                {
                    // logique d'attaque
                    nav.ResetPath();
                    DealAutoDamage(Target);
                }
                else
                {
                    nav.SetDestination(Target.transform.position);
                }
            }
            // if to change later (it should shoot when it's a tower, a player or a minion)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                
                    Vector3 direction = (targetPosition - transform.position).normalized;
                    Vector3 spawnPosition = transform.position + direction * 1.0f;

                    // Instantiate the projectile   
                    GameObject proj = Realtime.Instantiate("Projectile", spawnPosition, Quaternion.LookRotation(targetPosition - transform.position), preventOwnershipTakeover: true, useInstance: _realtime);

                    // Set the direction of the projectile
                    ProjectileScript projScript = proj.GetComponent<ProjectileScript>();
                    projScript.SetDirection((targetPosition - transform.position).normalized);
                }
            }
        }

    }
}
