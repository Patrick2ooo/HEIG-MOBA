using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    
    private int nbChars;
    public GameObject[] characters = GameObject.FindGameObjectsWithTag("player");
    
    public Transform[] oldCharactersTransform;
    
    
    public Animator[] animators;

    // Update is called once per frame
    void Update()
    {
        characters = GameObject.FindGameObjectsWithTag("Player");
        
        if (oldCharactersTransform == null || nbChars != characters.Length)
        {
            // copy transforms
            nbChars = characters.Length;
        
            animators = new Animator[characters.Length];
            
            oldCharactersTransform = new Transform[characters.Length];
        
            for (int i = 0; i < characters.Length; i++)
            {
                animators[i] = characters[i].GetComponentInChildren<Animator>();
            }
            
        }


        for (int i = 0; i < nbChars; i++)
        {
            if (CheckTransformsIdentical(characters[i].transform, oldCharactersTransform[i]))
            {
                animators[i].SetBool("isWalking", true);
            }
            else
            {
                characters[i].GetComponentInChildren<Animator>();
                animators[i].SetBool("isWalking", false);
                
            }
            
            // store new transform to check next update
            oldCharactersTransform[i] = characters[i].transform;
        }
        
    }

    private bool CheckTransformsIdentical(Transform t1, Transform t2)
    {
        // Compare positions
        if (t1.position != t2.position)
        {
            return false;
        }

        // Compare rotations
        if (t1.rotation != t2.rotation)
        {
            return false;
        }

        // Compare scales
        if (t1.localScale != t2.localScale)
        {
            return false;
        }

        return true; // All properties are identical
    }
    
}
