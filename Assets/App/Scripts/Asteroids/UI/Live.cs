using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Live : MonoBehaviour
{

    public static float live = 100;
    public TextMeshPro liveText;


    //Leben des Spieles anzeigen und aktualisieren
    void Update()
    {
        liveText.SetText("Leben: " + live + "%"); 
    }
}
