using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyLevel : MonoBehaviour
{


    public Slider slider;
    public static int difficultiy = 0;


    // Den Schwierigkeitsgrad aktualisieren und speichern
    void Update()
    {
        difficultiy = (int)slider.value;
    }
}
