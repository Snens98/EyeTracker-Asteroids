using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textTime : MonoBehaviour
{

    public TextMeshPro t; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (score.time > 0)
        {
            t.text = "Zeit: " + (int)score.time;
        }
        else
        {
            t.text = "Zeit: 0";

        }
    }
}
