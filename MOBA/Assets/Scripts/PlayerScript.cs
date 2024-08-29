using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerScript : Character
{
    public Camera mainCamera;
    private const int MapLayer = 3, UILayer = 5, CharactersLayer = 6;
    public GameObject icon;
    public NavMeshAgent nav;
    private static readonly Vector3 Offset = new(0, 0.1f, 0);
    private RealtimeView _view;
    private bool _nextAttackBuffed, _boostedStats;
    private float _cTimer;

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

    public override void SpellA()
    {
        _nextAttackBuffed = true;
    }

    public override void SpellB()
    {
        model.health = Math.Max(model.health + 50, model.maxHealth);
    }

    public override void SpellC()
    {
        model.physDef += 20;
        model.magDef += 20;
        model.moveSpeed *= 1.1f;
        _boostedStats = true;
        _cTimer = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        model.attackRange = 1.0f;
        model.attack = 2;
        model.attackPerLevel = 1;
        if (_view.isOwnedLocallyInHierarchy)
        {
            GetComponent<RealtimeTransform>().RequestOwnership();
        }
        radius = 0.5f;
    }

    protected override void UpdateHealth(Attributes updated, float health)
    {
        if (health <= 0)
        {
            Realtime.Destroy(transform.parent.gameObject);
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
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    var eventData = new PointerEventData(EventSystem.current);
                    eventData.position = Input.mousePosition;
                    var results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, results);
                    if (results.Count(r => r.gameObject.layer == UILayer) == 0)
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
                                if (Target.GetSide() == model.side)
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
            }
            if (Target)
            {
                if (Vector3.Distance(transform.position, Target.transform.position) - radius - Target.radius <= model.attackRange)
                {
                    // logique d'attaque
                    nav.ResetPath();
                    DealAutoDamage(Target);
                    if (_nextAttackBuffed) _nextAttackBuffed = false;
                }
                else
                {
                    nav.SetDestination(Target.transform.position);
                }
            }
        }

        if (_boostedStats)
        {
            if (_cTimer > 0)
            {
                _cTimer -= Time.deltaTime;
            }

            if (_cTimer <= 0)
            {
                _boostedStats = false;
                model.physDef -= 20;
                model.magDef -= 20;
                model.moveSpeed /= 1.1f;
            }
        }
        
    }

    protected override void DealAutoDamage(Entity target)
    {
        target.ReceiveDamage(this, model.attack + (_nextAttackBuffed ? 10 : 0), 0, model.physPen, model.magPen, model.critChance, model.critMult);
    }
}
