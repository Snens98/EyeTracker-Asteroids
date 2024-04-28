using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject menu;

    void Update()
    {

        //Wenn man auf den Start-Button drückt, soll das Menü deaktiviert werden
        if (StartButton.startGame)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
}
