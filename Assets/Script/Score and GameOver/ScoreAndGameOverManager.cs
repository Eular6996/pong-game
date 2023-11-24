using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndGameOverManager : MonoBehaviour
{
    private bool m_player1Win = false;
    public bool m_player2Win = false;

    private int player1Score;
    private int player2Score;

    [SerializeField] private Text m_player1Score;
    [SerializeField] private Text m_player2Score;

    [SerializeField] private int m_WinScore;

    [SerializeField] private GameObject[] Player1WinTextAndButtons;
    [SerializeField] private GameObject[] Player2WinTextAndButtons;

    private bool isGameOver = false;
    //getter and setter
    public bool Player1Win
    {
        get { return m_player1Win; }
        set { m_player1Win = value; }
    }
    public bool Player2Win
    {
        get { return m_player2Win; }
        set { m_player2Win = value; }
    }
    public bool IsGameOver
    {
        get { return isGameOver;}
        set { isGameOver = value; }
    }

    void Start()
    {
        StartGame();
    }

    // Update is called once per frame

    public void UpdatePlayerScore(string playerTag)
    {
        //If Non of player Win then only update score
        if(!Player1Win && !Player2Win)
        {
            if (playerTag == "Player1")
                player1Score += 1;

            else if (playerTag == "Player2")
                player2Score += 1;

            m_player1Score.text = player1Score.ToString();
            m_player2Score.text = player2Score.ToString();

            CheckGameOver();
        }
    }

    //If Player get winScore Then game Over
    void CheckGameOver()
    {
        if (player1Score >= m_WinScore)
        {
            Player1WinGame();
            isGameOver = true;
        }

        else if (player2Score >= m_WinScore)
        {
            Player2WinGame();
            isGameOver = true;
        }

        GameOver();
    }
    
  
    //Player1 Winning Text and PlayAgain pop-up
    void Player1WinGame()
    {
        Player1Win = true;
        foreach (GameObject obj in Player1WinTextAndButtons)
        {
            obj.SetActive(true);
        }
    }

  
    //Player1 Winning Text and PlayAgain pop-up
    void Player2WinGame()
    {
        Player2Win = true;
        foreach (GameObject obj in Player2WinTextAndButtons)
        {
            obj.SetActive(true);
        }
    }

   void GameOver()
    {
        if(isGameOver)
        {
            AudioManager.instance.PlaySFX("GameOver");
        }   
    }

    //start the Game After Play Again Pressed
    public void StartGame()
    {
        //reset all value
        isGameOver = false;
        Player1Win = false;
        Player2Win = false;

        player1Score = 0;
        player2Score = 0;

        m_player1Score.text = player1Score.ToString();
        m_player2Score.text = player2Score.ToString();


        //disable PlayAgain and winning text 
        foreach (GameObject obj in Player1WinTextAndButtons)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in Player2WinTextAndButtons)
        {
            obj.SetActive(false);
        }
    }

}
