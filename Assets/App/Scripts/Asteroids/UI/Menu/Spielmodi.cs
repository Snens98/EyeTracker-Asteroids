using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tobii.XR;
using UnityEngine;

public class Spielmodi : MonoBehaviour
{

    public TextMeshPro DropDown;
    public static int spielmodi = 0;


 
    //Wenn man im Dropdown-Menü den Spielmodus ändert, wird hier der Spielmodus gesetzt
    public void bla(int val)
    {
        //Eye Tracker
        if (val == 0)
        {
            spielmodi = 0;
        }
        //Head Tracker
        if (val == 1)
        {
            spielmodi = 1;
        }
        //Controller Tracker
        if (val == 2)
        {
            spielmodi = 2;
        }
    }
}
