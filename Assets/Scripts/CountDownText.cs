using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class NewBehaviourScript : MonoBehaviour
{
    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;
    Text countdown;
    void OnEnable()
    {
        countdown = GetComponent<Text>();
        countdown.text = "3";
        StartCoroutine("Coundown");


    }
    IEnumerator Countdown()
    {
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            countdown.text = (count - 1).ToString();
            yield return new WaitForSeconds(1);
        }
        OnCountdownFinished();

    }
}