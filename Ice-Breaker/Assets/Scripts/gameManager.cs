using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager _instance;
    public int player1Score;
    public int player2Score;
    public bool isGameActive;

    #region Singleton

    public static gameManager instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<gameManager>();
            }
            return _instance;
        }
    }
    //Game manager is persistent across scenes
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player1Score = 0;
        player2Score = 0;
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            // Game logic goes here
        }
    }

    public void AddScore(int playerNumber, int score)
    {
        if (playerNumber == 1)
        {
            player1Score += score;
        }
        else if (playerNumber == 2)
        {
            player2Score += score;
        }
    }

    public void EndGame()
    {
        isGameActive = false;
    }
}
