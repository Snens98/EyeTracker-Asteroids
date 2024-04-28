using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textPoints : MonoBehaviour
{

    public TextMeshPro t; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t.text = "Punkte: " + (int)score.points; 
    }
}
