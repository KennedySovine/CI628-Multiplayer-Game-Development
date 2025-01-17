using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    [Header("UI Elements")]
    public GameObject turnIndicator;

    private Text turnIndicatorText;

    private ulong currentPlayerId = 0;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        turnIndicatorText = turnIndicator.GetComponent<Text>();
        UpdateTurnIndicator();
    }

    void Update()
    {
        if (currentPlayerId != gameManager.currentPlayerId)
        {
            currentPlayerId = gameManager.currentPlayerId;
            UpdateTurnIndicator();
        }
    }

    public void UpdateTurnIndicator()
    {
        if (gameManager.isGameActive)
        {
            if (currentPlayerId == gameManager.currentPlayerId)
            {
                turnIndicatorText.text = "It's your turn";
            }
            else
            {
                turnIndicatorText.text = "It is the other player's turn";
            }
        }
        else
        {
            turnIndicator.SetActive(false);
        }
    }
}
