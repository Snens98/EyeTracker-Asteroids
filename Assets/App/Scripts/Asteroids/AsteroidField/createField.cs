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

                                        //max. Asteroiden,     spawn Timer,    Anfangsasteroiden,     Winkel,         spawnHöhe,      timer1,         timer2,         Spielzeit
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






    //Timer für den start setzen

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

        //Wenn man den Menü-Button des Kontroller drückt, sollen alle Asteroiden gelöscht werden
        if ((input.isMenuRightPressed || input.isMenuLeftPressed || Live.live <= 0) && !deleted)
        {
           deleteAsteroidField();
           deleted = true;
            red.SetActive(false);

        }

        //Wenn man ein neues Spiel startet, soll alles vorher zurückgesetzt werden
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



        // Die Spielzeit wird runter gezählt
        score.time -= Time.deltaTime;



        //Jede x Sek. kann ein zusätzlicher Asteroid auf dem Feld sein
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


        //Zählt die festgelegte Zeit runter, indem ein ein neuer Asteroid spawnt
        timer -= Time.deltaTime;


        //Wenn timer zu ende und Feld nicht voll, Asteroid auf Feld hinzufügen 
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




    //Überprüfen, ob Timer beendet ist
    private bool isTimerEnded(float t)
    {
        return t <= 0.0f;
    }


    //Überprüfen, ob das Feld voll ist
    private bool isFieldFull()
    {
        return asteroidList.Count >= asteroidCount;
    }





    //Einen neuen Asteroid erstellen
    public void SpawnAsteroids(int fov, int radius, float posZ)
    {
        Vector3 spawn = new Vector3();

        //Hier wird eine zufällige Position anhand den angegeben Paramatern festgelegt 
        int value = 360 - fov;
        double pos = Random.Range(0 - 90 + (value / 2), 360 - 90 - (value / 2)) * Mathf.Deg2Rad;
        spawn.Set((float)(Math.Cos(pos) * radius), Random.Range(-posZ, posZ), (float)(Math.Sin(pos) * radius) + 0);

        // Neuen Asteroiden erstellen
        GameObject obj = Instantiate(asteroid, spawn, Random.rotation);
        obj.transform.localScale = (Vector3.one * 2 * Random.Range(2.0f, 8.0f));


        //Asteroiden in Liste hinzufügen
        asteroidList.Add(obj);

        //Nachdem der Asteroid hinzugefügt wurde, Spawntimer für den Asteroiden zurücksetzen
        timer = spawnTimer;
    }





    private Outline outline;

    //Alle Asteroiden löschen und die Liste der Asteroiden leeren. Spiel beenden.

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


    //Das gesamte Spiel zurücksetzen
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








    //Werte für das Asteridenfeld festlegen für die Schwierigkeitsgrade
    private void setSettingsOfAsteroidField(int asteroid_Count,
        float spawn_Timer, int asteroids_AtBeginning, int angle, int spawnHeight,
        int timer1, int timer2, int gameTime)
    {
        asteroidCount = asteroid_Count;                 //Wie viele Asteroiden gleichzeitig im Spiel sein dürfen
        spawnTimer = spawn_Timer;                       //Nach welcher zeit ein neuer Asteroid erstellt wird
        asteroidsAtBeginning = asteroids_AtBeginning;   //Wie viele ASteroiden am anfang erstellt sind
        fov = angle;                                    //In welchen Winel zum Spieler die Asteroiden spawnwn
        minRadius = 500;                                //Minimaler Spawn-Radius
        maxRadius = 2500;                               //Maximaler Spawn-Radius
        height = spawnHeight;                           //In welcher Höhe die Asteroiden spawen können
        score.time = gameTime;                          //Die Spielzeit

        timer = spawnTimer;
        timer5.setTimer(timer1);
        spawn.setTimer(timer2);

    }
}