using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class StartButton : MonoBehaviour
{


    public GameObject asteroid;
    public GameObject asteroidField;




    public static bool startGame = false;

    //Wenn man auf den Startbutton klickt, soll das Spiel gestartet werden
    public void onClickButton()
    {
        //if (!audioSource.isPlaying)
        {
            Reset.reset = true;

            /*
            asteroid.GetComponent<move>().enabled = false;
            asteroidField.GetComponent<createField>().enabled = false;

            asteroid.GetComponent<move>().enabled = true;
            asteroidField.GetComponent<createField>().enabled = true;
            */

            startGame = true;
            createField.SoundhasPlayed = false;

        }

        playSound(clip);

    }




    //Zum testen
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Reset.reset = true;
            startGame = true;
        }
    }




    public static bool SoundhasPlayed = false;
    public AudioSource audioSource;
    public AudioClip clip;




    //Sound abspielen
    private void playSound(AudioClip src)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(src);
        } 
    }
}
