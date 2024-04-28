using System;
using UnityEngine;
using Valve.VR.Extras;
using Random = UnityEngine.Random;


public class move : MonoBehaviour
{

    [HideInInspector] public float minRotationSpeed = 2f;
    [HideInInspector] public float maxRotationSpeed = 20f;
    [HideInInspector] public float minVelocity = 0.1f;
    [HideInInspector] public float maxVelocity = 0.2f;
    [HideInInspector] public float targetRadius = 0.5f;

    [HideInInspector] public float velocity;
    private float RotationSpeed;
    private Rigidbody rb;

    int rp;




    private void OnEnable()
    {

        Vector3 forceVec = new Vector3(0, 0, 0);

        switch (DifficultyLevel.difficultiy)
        {

            case 0: //Leicht


                                        // min. Geschwindigkeit,        max. Geschwindigkeit,           Zielradius)

                setAsteroidMovingSettings(          15f,                          22f,                       20);
                break;

            case 1: //Mittel

                setAsteroidMovingSettings(18f, 26f, 15);
                break;


            case 2: //Schwer

                setAsteroidMovingSettings(18f, 32f, 10);
                break;

            default:

                break;
        }
    }









    void Start()
    {
        rp = Random.Range(0, 100);
        RotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        velocity = Random.Range(minVelocity, maxVelocity);
        rb = GetComponent<Rigidbody>();
        Vector3 forceVec = Camera.main.transform.position - this.transform.position;
        forceVec = forceVec + new Vector3(Random.Range(-targetRadius, targetRadius),
            Random.Range(-targetRadius, targetRadius), Random.Range(-targetRadius, targetRadius));

        rb.AddForce(Vector3.Normalize(forceVec) * velocity, ForceMode.Impulse);
    }






    void Update()
    {
        this.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        replaceAsteroid((this.transform.position.z + rp) < Camera.main.transform.position.z);

        //Wenn die Geschwindigkeit zu klein ist z.B nach einer Kollision, wird der Asteroid entfert.
        //checkVelocity();
    }




    // Wenn die Asteroiden neu gesetzt werden sollen, wird die Geschwindigkeit zurückgesetzt
    public void resetVelocity()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }





    //Wenn die Geschwindigkeit zu klein ist z.B nach einer Kollision, wird der Asteroid entfert.
    private void checkVelocity()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < 4.0)
        {
            createField.asteroidList.Remove(this.gameObject);
            DestroyImmediate(this.gameObject);
        }
    }






    // Wenn die Asteroiden neu gesetzt werden, bekommen die einen neuen Start-Inpuls
    public void addForce()
    {

        //Vektor, der vom Asteroiden zur Kamera zeigt
        Vector3 forceVec = Camera.main.transform.position - this.transform.position;

        //Wohin der Asteroid fliegen soll
        forceVec = forceVec + new Vector3(Random.Range(-targetRadius, targetRadius),
            Random.Range(-targetRadius, targetRadius), Random.Range(-targetRadius, targetRadius));

        //Asteroid bekommt einen Impuls, um sich zu bewegen
        rb.AddForce(Vector3.Normalize(forceVec) * velocity, ForceMode.Impulse);
    }






    //Asteroid an einer neuen zufälligen Position setzen
    public void replaceAsteroid(bool b)
    {
        int fov = GameObject.Find("AsteroidField").GetComponent<createField>().fov;
        int minRadius = GameObject.Find("AsteroidField").GetComponent<createField>().minRadius;
        int maxRadius = GameObject.Find("AsteroidField").GetComponent<createField>().maxRadius;
        int height = GameObject.Find("AsteroidField").GetComponent<createField>().height;
        int radius = Random.Range(minRadius, maxRadius);

        //Wenn Asteroid hinter der Kamera verschwindet
        if (b)
        {

            //Wenn die Spielzeit um ist
            if (score.time <= 0)
            {

                // Asteroiden aus Liste und Feld entfernen
                createField.asteroidList.Remove(this.gameObject);
                DestroyImmediate(this.gameObject);
            }
            else
            {
                // Geschwindigkeit zurücksetzen
                resetVelocity();


                // Neue Position im festgelegen Bereich berechnen
                int value = 360 - fov;
                Vector3 replacePos = new Vector3();
                double pos = Random.Range(0 - 90 + (value / 2), 360 - 90 - (value / 2)) * Mathf.Deg2Rad;
                replacePos.Set((float)(Math.Cos(pos) * radius), Random.Range(-height, height), (float)(Math.Sin(pos) * radius) + 0);

                //Asteroid wieder nach hinten (vor der Kamera) setzen
                this.transform.position = replacePos;

                // Neuen Asteroid wieder einen Start-Impuls geben
                addForce();
            }
        }
    }



    //Werte für das Verhalten des Asteroids für die Schwierigkeitsgrade festlegen
    private void setAsteroidMovingSettings(float min_Velocity, float max_Velocity, float target_Radius)
    {
        Vector3 forceVec = new Vector3(0, 0, 0);

        minRotationSpeed = 2f;
        maxRotationSpeed = 20f;
        minVelocity = min_Velocity;
        maxVelocity = max_Velocity;
        targetRadius = target_Radius;

        RotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        velocity = Random.Range(minVelocity, maxVelocity);

        rb = GetComponent<Rigidbody>();

        forceVec = Camera.main.transform.position - this.transform.position;

        forceVec = forceVec + new Vector3(Random.Range(-targetRadius, targetRadius),
        Random.Range(-targetRadius, targetRadius), Random.Range(-targetRadius, targetRadius));

        rb.AddForce(Vector3.Normalize(forceVec) * velocity, ForceMode.Impulse);
    }
}
