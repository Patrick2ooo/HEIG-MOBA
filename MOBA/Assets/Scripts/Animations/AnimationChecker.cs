using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    
    private int nbChars;
    public GameObject[] characters;
    public Vector3[] oldPositions;
    public Quaternion[] oldRotations;
    public Vector3[] oldScales;
    
    public Animator[] animators;

    // Update is called once per frame
    void Update()
    {
        // Get characters tagged as "Player"
        characters = GameObject.FindGameObjectsWithTag("Player");

        // If it's the first time we're running Update or if the number of characters has changed
        if (oldPositions == null || characters.Length != nbChars)
        {
            nbChars = characters.Length;

            // Initialize arrays to store the transforms and animators
            oldPositions = new Vector3[nbChars];
            oldRotations = new Quaternion[nbChars];
            oldScales = new Vector3[nbChars];
            animators = new Animator[nbChars];

            // Populate the arrays with the current characters' data
            for (int i = 0; i < nbChars; i++)
            {
                animators[i] = characters[i].GetComponentInChildren<Animator>();

                // Store the initial positions, rotations, and scales
                oldPositions[i] = characters[i].transform.position;
                oldRotations[i] = characters[i].transform.rotation;
                oldScales[i] = characters[i].transform.localScale;
            }
        }

        // Iterate over characters and check if they have moved
        for (int i = 0; i < nbChars; i++)
        {
            Transform currentTransform = characters[i].transform;

            if (CheckTransformsIdentical(currentTransform.position, currentTransform.rotation, currentTransform.localScale,
                                          oldPositions[i], oldRotations[i], oldScales[i]))
            {
                animators[i].SetBool("isWalking", false);
            }
            else
            {
                animators[i].SetBool("isWalking", true);
            }

            // Update the old transforms to the current values for the next frame
            oldPositions[i] = currentTransform.position;
            oldRotations[i] = currentTransform.rotation;
            oldScales[i] = currentTransform.localScale;
        }
    }

    private bool CheckTransformsIdentical(Vector3 pos1, Quaternion rot1, Vector3 scale1,
                                          Vector3 pos2, Quaternion rot2, Vector3 scale2)
    {
        return pos1 == pos2 && rot1 == rot2 && scale1 == scale2;
    }
    
}
