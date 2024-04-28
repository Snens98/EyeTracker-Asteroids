using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{


    public AudioSource audioSource;
    public AudioClip clip;


    void Update()
    {
        playSound(clip);
    }

    //Hintergrundmusik abspielen
    private void playSound(AudioClip src)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(src);
        }
    }
}
