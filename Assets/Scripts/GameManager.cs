using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static NewBehaviourScript;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;
    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountdownPage;
    //public GameObject CountdownText;
    public Text ScoreText;
   // public Text CountdownText;
    
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
     void OnEnable()
    {
       CountDownText.OnCountdownFinished += OnCountdownFinished; 
        TapController.OnplayerDied += OnPlayerDied;
        TapController.OnplayerScored += OnPlayerScored;

    }
     void OnDisable()
    { 
       CountDownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnplayerDied -= OnPlayerDied;
        TapController.OnplayerScored -= OnPlayerScored;

    }
    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
    void OnPlayerDied()
    { 
        gameOver = true;
        int savedscore = PlayerPrefs.GetInt("HighScore");
      if(score > savedscore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }
    void OnPlayerScored()
    {
        score++;
        ScoreText.text = score.ToString();
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
