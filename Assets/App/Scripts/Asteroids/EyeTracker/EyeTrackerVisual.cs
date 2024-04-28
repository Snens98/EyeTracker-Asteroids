using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTrackerVisual : MonoBehaviour
{

    public GameObject visual;
    public Toggle toggle;

    void Update()
    {

        if(Spielmodi.spielmodi == 2 || (Spielmodi.spielmodi == 0 && !toggle.isOn))
        {
            visual.SetActive(false);
        }
        else
        {
            visual.SetActive(true);

        }
    }
}
