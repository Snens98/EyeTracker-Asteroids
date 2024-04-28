using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class createField : MonoBehaviour
{

    public GameObject asteroid;
    public GameObject red;

    [HideInInspector] public int asteroidCount = 50;
    [HideInInspector] public float spawnTimer = 10.0f;
    [HideInInspector] public int fov;
    [HideInInspector] public int minRadius = 500;
    [HideInInspector] public int maxRadius = 2500;
    [HideInInspector] public int height = 200;
    [HideInInspector] public int asteroidsAtBeginning = 25;


    private float timer;
    public static List<GameObject> asteroidList = new List<GameObject>();
    private float playtime;


    Timer timer5 = new Timer();
    Timer spawn = new Timer();






    //Schwierigkeitslevel einstellen
    private void OnEnable()
    {
        red.SetActive(false);
        switch (DifficultyLevel.difficultiy)
        {
            case 0: //Leicht

                                        //max. Asteroiden,     spawn Timer,    Anfangsasteroiden,     Winkel,         spawnH�he,      timer1,         timer2,         Spielzeit
                setSettingsOfAsteroidField(     80             , 10f               , 30,               70,             250,            20,             30,             150);
                break;

            case 1: //Mittel

                setSettingsOfAsteroidField(100, 8f, 50, 80, 300, 15, 25, 180);
                break;

            case 2: //Schwer

                setSettingsOfAsteroidField(150, 8f, 60, 90, 400, 10, 20, 200);
                break;

            default:

                break;
        }
    }






    //Timer f�r den start setzen

    void Start()
    {
        timer = spawnTimer;

        timer5.setTimer(10);
        spawn.setTimer(20);

        playtime = score.time;

    }





    public static bool SoundhasPlayed = false;
    public AudioSource audioSource;
    public AudioClip clip;



    //Einen Sound abspielen, wenn das Spiel vorbei ist
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




    bool deleted = false;

    void Update()
    {

        //Wenn man den Men�-Button des Kontroller dr�ckt, sollen alle Asteroiden gel�scht werden
        if ((input.isMenuRightPressed || input.isMenuLeftPressed || Live.live <= 0) && !deleted)
        {
           deleteAsteroidField();
           deleted = true;
            red.SetActive(false);

        }

        //Wenn man ein neues Spiel startet, soll alles vorher zur�ckgesetzt werden
        if (StartButton.startGame && deleted)
        {
            resetGame();
            red.SetActive(false);

        }


        //Zum testen
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartButton.startGame = false;
            deleteAsteroidField();
            deleted = true;
            red.SetActive(false);

        }


        //Wenn das Spiel noch nicht gestartet wurde, soll hier abgebrochen werden
        if (!StartButton.startGame)
        {
            return;
        }


        // Wenn keine Asteroiden am Ende mehr da sind, soll das Spiel stoppen
        if (asteroidList.Count == 0 && score.time <= 0)
        {
            playSound(clip);
            StartButton.startGame = false;
            deleteAsteroidField();
            deleted = true;
            red.SetActive(false);


        }



        // Die Spielzeit wird runter gez�hlt
        score.time -= Time.deltaTime;



        //Jede x Sek. kann ein zus�tzlicher Asteroid auf dem Feld sein
        if (timer5.delay())
        {
            asteroidCount++;
        }


        // Alle x Sek. verringert sich der Spawntimer der Asteroiden um 0.5 Sekunden
        if (spawn.delay())
        {
            if (spawnTimer > 0.5)
                spawnTimer -= 0.5f;
        }


        //Z�hlt die festgelegte Zeit runter, indem ein ein neuer Asteroid spawnt
        timer -= Time.deltaTime;


        //Wenn timer zu ende und Feld nicht voll, Asteroid auf Feld hinzuf�gen 
        if (isTimerEnded(timer) && !isFieldFull() && score.time > 0)
        {
            SpawnAsteroids(fov, Random.Range(minRadius, maxRadius), height);
        }


        //Am Anfang eine festgelegte Anzahl von Asteroiden, die sofort spawnen

        if (asteroidList.Count <= asteroidsAtBeginning && score.time > 0)
        {
            SpawnAsteroids(fov, Random.Range(minRadius, maxRadius), height);
        }
    }




    //�berpr�fen, ob Timer beendet ist
    private bool isTimerEnded(float t)
    {
        return t <= 0.0f;
    }


    //�berpr�fen, ob das Feld voll ist
    private bool isFieldFull()
    {
        return asteroidList.Count >= asteroidCount;
    }





    //Einen neuen Asteroid erstellen
    public void SpawnAsteroids(int fov, int radius, float posZ)
    {
        Vector3 spawn = new Vector3();

        //Hier wird eine zuf�llige Position anhand den angegeben Paramatern festgelegt 
        int value = 360 - fov;
        double pos = Random.Range(0 - 90 + (value / 2), 360 - 90 - (value / 2)) * Mathf.Deg2Rad;
        spawn.Set((float)(Math.Cos(pos) * radius), Random.Range(-posZ, posZ), (float)(Math.Sin(pos) * radius) + 0);

        // Neuen Asteroiden erstellen
        GameObject obj = Instantiate(asteroid, spawn, Random.rotation);
        obj.transform.localScale = (Vector3.one * 2 * Random.Range(2.0f, 8.0f));


        //Asteroiden in Liste hinzuf�gen
        asteroidList.Add(obj);

        //Nachdem der Asteroid hinzugef�gt wurde, Spawntimer f�r den Asteroiden zur�cksetzen
        timer = spawnTimer;
    }





    private Outline outline;

    //Alle Asteroiden l�schen und die Liste der Asteroiden leeren. Spiel beenden.

    private void deleteAsteroidField()
    {
        foreach (GameObject obj in asteroidList)
        {
            outline = obj.GetComponent<Outline>();
            outline.enabled = false;
            Destroy(obj);
        }
        asteroidList.Clear();

        StartButton.startGame = false;
    }


    //Das gesamte Spiel zur�cksetzen
    private void resetGame()
    {
        foreach (GameObject obj in asteroidList)
        {
            outline = obj.GetComponent<Outline>();
            outline.enabled = false;
            Destroy(obj);
        }
        asteroidList.Clear();

        playtime = score.time;
        score.reset();
        Live.live = 100.0f;
        timer = spawnTimer;
        //StartButton.startGame = false;
        deleted = false;
    }








    //Werte f�r das Asteridenfeld festlegen f�r die Schwierigkeitsgrade
    private void setSettingsOfAsteroidField(int asteroid_Count,
        float spawn_Timer, int asteroids_AtBeginning, int angle, int spawnHeight,
        int timer1, int timer2, int gameTime)
    {
        asteroidCount = asteroid_Count;                 //Wie viele Asteroiden gleichzeitig im Spiel sein d�rfen
        spawnTimer = spawn_Timer;                       //Nach welcher zeit ein neuer Asteroid erstellt wird
        asteroidsAtBeginning = asteroids_AtBeginning;   //Wie viele ASteroiden am anfang erstellt sind
        fov = angle;                                    //In welchen Winel zum Spieler die Asteroiden spawnwn
        minRadius = 500;                                //Minimaler Spawn-Radius
        maxRadius = 2500;                               //Maximaler Spawn-Radius
        height = spawnHeight;                           //In welcher H�he die Asteroiden spawen k�nnen
        score.time = gameTime;                          //Die Spielzeit

        timer = spawnTimer;
        timer5.setTimer(timer1);
        spawn.setTimer(timer2);

    }
}