using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject minionPrefab;

    private float _spawnerTimer = 0;
    
    public Vector3 leftSideSpawner, rightSideSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _spawnerTimer += Time.deltaTime;
        if (_spawnerTimer > 10)
        {
            _spawnerTimer -= 10;
            for (int i = 0; i < 5; ++i)
            {
                MinionScript minionLeft = Instantiate(minionPrefab, leftSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                minionLeft.destination = rightSideSpawner;
                minionLeft.setSide(0);
                MinionScript minionRight = Instantiate(minionPrefab, rightSideSpawner, Quaternion.identity).GetComponent<MinionScript>();
                minionRight.destination = leftSideSpawner;
                minionRight.setSide(1);
            }
        }
    }
}
