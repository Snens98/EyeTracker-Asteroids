using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{ 
    public GameObject asteroid;
    public GameObject asteroidField;
    public static bool reset = false;

    void Update()
    {
        if (reset)
        {
            asteroid.GetComponent<move>().enabled = false;
            asteroidField.GetComponent<createField>().enabled = false;

            asteroid.GetComponent<move>().enabled = true;
            asteroidField.GetComponent<createField>().enabled = true;

            reset = false;
        }
    }
}
