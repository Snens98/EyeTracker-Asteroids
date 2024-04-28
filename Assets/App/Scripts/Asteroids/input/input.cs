using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class input : MonoBehaviour
{


    public GameObject red; 

    // Hier kann auf die Inputs zugegriffen werden
    public static bool isTriggerRightPressed = false;   //Der Trigger-Button am rechten Kontroller
    public static bool isTriggerLeftPressed = false;    //Der Trigger-Button am linken Kontroller

    public static bool isTriggerJumped = false;         //Trigger-Button rechts. wird nur kurz true bei Bet�tigung 
    public static bool isTriggerJumpLeft = false;       //Trigger-Button links. wird nur kurz true bei Bet�tigung


    public static bool isMenuRightPressed = false;      //Men�-Button rechts 
    public static bool isMenuLeftPressed = false;       //Men�-Button links



    private SteamVR_Action_Boolean inputTrigger;
    private SteamVR_Action_Boolean menuTrigger;


    void Start()
    {
        // Trigger-Button
        inputTrigger = SteamVR_Actions.default_GrabPinch;

        //Men�-Button
        menuTrigger = SteamVR_Actions.default_Teleport;

    }

    void Update()
    {


        // Reagiert beim loslassen des Triggers beim Rechten Controller
        isTriggerRightPressed = inputTrigger.GetStateDown(SteamVR_Input_Sources.RightHand);

        // Reagiert beim dr�cken des Triggers beim Rechten Controller
        isTriggerJumped = inputTrigger.GetStateUp(SteamVR_Input_Sources.RightHand);

        // Reagiert kurz beim dr�cken des Triggers beim linken Kontroller
        isTriggerJumpLeft = inputTrigger.GetStateUp(SteamVR_Input_Sources.LeftHand);

        // Reagiert beim dr�cken auf Men� rechts
        isMenuRightPressed = menuTrigger.GetStateDown(SteamVR_Input_Sources.RightHand);

        // Reagiert beim dr�cken auf Men� links
        isMenuLeftPressed = menuTrigger.GetStateDown(SteamVR_Input_Sources.LeftHand);


        //Wenn die Men�-Taste gedr�ckt ist, soll das Spiel gestoppt werden
        if (isMenuRightPressed || isMenuLeftPressed)
        {
            StartButton.startGame = false;
            red.SetActive(false);
        }
    }
}
