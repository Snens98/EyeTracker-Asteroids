using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.G2OM;
using Tobii.XR;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class EyeTracking : MonoBehaviour, IGazeFocusable
{

    private Renderer _renderer;
    private Color originalColor;
    public Outline outline;


    // Wie nah der Asteroid sein muss, damit er die Farbe ändert
    public int targetHighlightDistance = 200;

    //Die Farbe, um zu visualisieren, dass man den Asteroiden zerstören kann
    public Color highlightColor = Color.green;
    public float colorAlpha = 1.0f;

    float targetRadius;
    float velocity;
    Rigidbody rb;

    void Start()
    {

        // Auf die Variablen der erstellten Asteroiden zugreifen
        targetRadius = GameObject.Find("Cube(Clone)").GetComponent<move>().targetRadius;
        velocity = GameObject.Find("Cube(Clone)").GetComponent<move>().velocity;
        rb = GetComponent<Rigidbody>();


        // Transparenz des Materials setzen
       // highlightColor.a = colorAlpha;


        //Auf das Outline-Script von QuickOutline zugreifen
        outline = gameObject.GetComponent<Outline>();

        // Stadardmäßig sind alle Asteroiden nicht umrandet
        outline.enabled = false;


        // Auf den Renderer der Ateroiden zugreifen, um das Material zu ändern
        _renderer = GetComponent<Renderer>();

        // Orginalfarbe des Materials zwischenspeichern
        originalColor = _renderer.material.color;
    }







    //Sound abspielen, wenn der Asteroid zerstört wurde
    private void playSoundWrongDestroyed(AudioClip src)
    {
        //if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(src);
        }   
    }





    void Update()
    {

        //Distanz berechen von Asteroid und Kamera
        double dist = Vector3.Distance(this.transform.position, Camera.main.transform.position);

        // Wenn Asteroid nah genug
        if (dist <= targetHighlightDistance)
        {
            // andere Farbe
            _renderer.material.color = Color.green;
        }
        else
        {
            //Wenn nicht, dann orginal Farbe
            _renderer.material.color = originalColor;
        }

        playSoundOnDestroy();

        // Den Asteroiden zerstören und neu setzen, wenn man ihn anvisiert und den Trigger drückt
        replaceAsteroid(asteroidCanBeDestroyed());
        
    }





    public void resetVelocity()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }





    private void playSoundOnDestroy()
    {
        // Wird der Asteroid grade angeschaut? 
        bool isTargetedWithEye = outline.enabled;

        // Wird der Trigger Rechts gedrückt?
        bool trigger = input.isTriggerJumped;


        if (isTargetedWithEye && trigger && _renderer.material.color == highlightColor)
        {
            playSoundWrongDestroyed(clipDestroyed);
        }


        if (isTargetedWithEye && trigger && _renderer.material.color != highlightColor)
        {
            playSoundWrongDestroyed(clipWrongDestroyed);
        }
    }




    //Was passieren soll, wenn der Asteroid zeerstört werden kann
    private bool asteroidCanBeDestroyed()
    {

        // weiter nur dann, wenn es überhaupt Asteroiden auf dem Feld gibt
        if (GameObject.Find("Cube(Clone)") != null)
        {

            // Wird der Asteroid grade angeschaut? 
            bool isTargetedWithEye = outline.enabled;

            // Wird der Trigger Rechts gedrückt?
            bool trigger = input.isTriggerJumped;


            // Wenn mit Auge anvisiert, Trigger gedrückt, Ansteroid  NICHT markiert
            if (isTargetedWithEye && trigger && _renderer.material.color != highlightColor)
            {
                score.totalDestroyedAsteroids++; // Instagsamt zerstörte Asteroiden +1
                score.missClick++;               // Fehlklick +1
                Live.live-=4;                    // 3% Leben abziehen

                if (score.points > 0)
                {
                    score.points--;             // punkte -1
                }

                outline.enabled = false;
                return true;                    // return true, damit asteroid zerstört wird

            }


            // Wenn mit Auge anvisiert, Trigger gedrückt, Ansteroid als "Abschussbereit" markiert
            if (isTargetedWithEye && trigger && _renderer.material.color == highlightColor)
            {


                if (Live.live < 100.0)
                {
                    Live.live += 0.10f;                    // 0.5% Leben dazubekommen
                }

                score.totalDestroyedAsteroids++; // Instagsamt zerstörte Asteroiden +1

                score.correctlyDestroyedAsteroids++;    // Instagsamt zerstörte Asteroiden +1
                score.points++;                         // Punkte +1
                return true;                            // return true, damit asteroid zerstört wird
            }




            // Wenn rigger gedrückt und nicht mit Auge anvisiert
            if (trigger && !isTargetedWithEye)
            {

                //score.missClick++;  // Fehlklick +1
                return false;       // return false, damit asteroid zerstört nicht zerstört wird
            }
        }
        return false;
    }






    public void addForce()
    {

        //Vektor, der vom Asteroiden zur Kamera zeigt
        Vector3 forceVec = Camera.main.transform.position - this.transform.position;

        //Wohin der Asteroid fliegen soll
        forceVec = forceVec + new Vector3(Random.Range(-targetRadius, targetRadius), Random.Range(-targetRadius, targetRadius), Random.Range(-targetRadius, targetRadius));

        //Asteroid bekommt einen Impuls, um sich zu bewegen
        rb.AddForce(Vector3.Normalize(forceVec) * velocity, ForceMode.Impulse);
    }




    public void replaceAsteroid(bool b)
    {

        //Auf die Variablen aus dem createField-Script zugreifen
        int fov = GameObject.Find("AsteroidField").GetComponent<createField>().fov;
        int minRadius = GameObject.Find("AsteroidField").GetComponent<createField>().minRadius;
        int maxRadius = GameObject.Find("AsteroidField").GetComponent<createField>().maxRadius;
        int height = GameObject.Find("AsteroidField").GetComponent<createField>().height;


        //Wenn Asteroid zerstört wird
        if (b)
        {

            // Wenn die Spielzeit um ist
            if (score.time <= 0)
            {
                // zerstörten Asteroiden nicht neu setzen, sonden komplett löschen
                createField.asteroidList.Remove(this.gameObject);
                DestroyImmediate(this.gameObject);
            }
            else
            {
                // Geschwindigkeit zurücksetzen
                resetVelocity();


                // Neue Position berechnen
                int value = 360 - fov;
                Vector3 replacePos = new Vector3();
                double pos = Random.Range(0 - 90 + (value / 2), 360 - 90 - (value / 2)) * Mathf.Deg2Rad;
                replacePos.Set((float)(Math.Cos(pos) * Random.Range(minRadius, maxRadius)),
                    Random.Range(-height, height), (float)(Math.Sin(pos) * Random.Range(minRadius, maxRadius)) + 0);

                //Asteroid wieder nach hinten (vor der Kamera) setzen
                this.gameObject.transform.position = replacePos;

                // Neuen Start-Impuls geben
                addForce();
            }
        }
    }



    //Wenn man mit den Eye-Tracker auf den Asteroiden schaut, wird die Methode ausgeführt
    public void GazeFocusChanged(bool hasFocus)
    {

        if (Spielmodi.spielmodi < 2) //Spielmodus Eye-Tracker oder Head-Tracking
        {
            // Wenn der Asteroid angeschaut wird
            if (hasFocus)
            {
                SoundhasPlayed = false;
                playSound(clip);

                // Asteroid umranden
                outline.enabled = true;
            }

            // Wenn nicht 
            else if (!hasFocus)
            {
                SoundhasPlayed = true;

                // Umrandung entfernen
                outline.enabled = false;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
    }






    public static bool SoundhasPlayed = false;
    public static bool SoundhasPlayed2 = false;

    public AudioSource audioSource;
    public AudioClip clip;
    public AudioClip clipWrongDestroyed;
    public AudioClip clipDestroyed;




    //Sound abspielen
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
