using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    public float t = 0f;
    private float t2;

    public void setTimer(float timer)
    {
        t = timer;
        t2 = timer;
    }



    public bool delay()
    {

        t2 -= Time.deltaTime;


        if (isTimerEnded(t2))
        {
            t2 = t;
            return true;
        }
        return false;
    }


    private bool isTimerEnded(float t)
    {
        return t <= 0.0f;
    }

}
