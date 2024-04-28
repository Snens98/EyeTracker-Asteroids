using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{

    public static int totalDestroyedAsteroids = 0;
    public static int points = 0;
    public static int correctlyDestroyedAsteroids = 0;
    public static int missClick = 0;
    public static float time = 150;



    public static void reset()
    {
    totalDestroyedAsteroids = 0;
    points = 0;
    correctlyDestroyedAsteroids = 0;
    missClick = 0;

        if (DifficultyLevel.difficultiy == 0)
        {
            time = 150;
        }else if(DifficultyLevel.difficultiy == 1)
        {
            time = 180;
        }
        else if(DifficultyLevel.difficultiy == 2)
        {
            time = 200;
        }
    }
}
