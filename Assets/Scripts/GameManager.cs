using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;
    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountdownPage;
    public Text ScoreText;
    
        enum PageState { 
        None,
        Start,
         GameOver,
         Countdown
     }
    int score = 0;
    bool gameOver = false;
    public bool GameOver { get { return gameOver; } }

     void Awake()
    {
        Instance = this;
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(false);
                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(true);
                break;
        }
    }
    public void ConfirmedGameOver()
    {
        // activiated when replay button is hit
        OnGameOverConfirmed();
        ScoreText.text = "0";
        SetPageState(PageState.Start);
    }
    public void StartGame()
    {
        SetPageState(PageState.Countdown);


    }

}
