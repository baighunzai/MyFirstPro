using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class HighScoreText : MonoBehaviour
{

    Text highscore;
     void Start()

    {
        highscore = GetComponent<Text>();
        highscore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }


}
