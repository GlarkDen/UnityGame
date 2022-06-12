using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesTaskGame : MonoBehaviour
{
    public Text timer;

    private void Start()
    {
        Timer.timer = timer;
        Timer.stopTimer = StopTimer;
        Timer.Start(new Clock(minutes:10));
    }

    public void StopTimer()
    {

    }
}
