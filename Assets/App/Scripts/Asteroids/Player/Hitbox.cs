using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Hitbox : MonoBehaviour
{


    public GameObject red;


    //Was passieren soll, wenn ein Asteroid mit der Kollisionsbox des Spieles kollidiert
    private void OnTriggerEnter(Collider other)
    {
        red.SetActive(true);
        SoundhasPlayed = false;
        playSound(clip);                //Sound abspielen


        //Asteroid zerstören
        //createField.asteroidList.Remove(other.gameObject);
        //Destroy(other.gameObject);

        //Leben reduzieren
        int modi = Spielmodi.spielmodi > -1 ? Spielmodi.spielmodi : 0;

       if ((Live.live - (4.0f * (modi + 1.0f))) < 0.0)
        {
            Live.live = 0.0f;
            red.SetActive(false);

            return;
        }
        Live.live -= 4.0f * (modi + 1.0f);

        //Leicht:   -4%
        //Mittel:   -8%
        //Schwer:   -12%
    }



    private void OnTriggerExit(Collider other)
    {
        red.SetActive(false);

    }



    public static bool SoundhasPlayed = false;
    public AudioSource audioSource;
    public AudioClip clip;


    //Einen Sound abspielen, wenn man vom Asteroiden getroffen wurde
    private void playSound(AudioClip src)
    {
        if (!SoundhasPlayed)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(src);
                SoundhasPlayed = true;
            }

        }
    }
}
