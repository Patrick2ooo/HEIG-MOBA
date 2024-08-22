using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PlayerScript : Character
{
    public Camera mainCamera;
    private const int MapLayer = 3, CharactersLayer = 6;
    public GameObject icon;
    public NavMeshAgent nav;
    private static readonly Vector3 Offset = new Vector3(0, 0.1f, 0);

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
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
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
    }
}
