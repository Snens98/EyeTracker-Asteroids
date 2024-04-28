using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsText : MonoBehaviour
{


    public TextMeshPro text;
    public TextMeshPro text2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        string s = DifficultyLevel.difficultiy == 0 ? "Leicht" : DifficultyLevel.difficultiy == 1 ? "Mittel" : "Schwer";
        string sp = Spielmodi.spielmodi == 0 ? "Eye-Tracking" : Spielmodi.spielmodi == 1 ? "Head-Tracking" : Spielmodi.spielmodi == 2? "Controller" : "Eye-Tracking";
        string t = (int)score.time <= 0 ? "0" : (int)score.time + "";

        text.SetText("Zerstört insgesamt:" +
            "\r\nPunkte:" +
            "\r\nFehler:" +
            "\r\nLeben:" +
            "\r\nVerbleibende Zeit:" +
            "\r\nSchwierigkeit:" +
            "\r\nSpielModi:");

        text2.SetText(score.totalDestroyedAsteroids +
            "\r\n" + score.points +
            "\r\n" + score.missClick +
            "\r\n" + Live.live + "%" +
            "\r\n" + t +
            "\r\n" + s +
            "\r\n" + sp);
    }
}
