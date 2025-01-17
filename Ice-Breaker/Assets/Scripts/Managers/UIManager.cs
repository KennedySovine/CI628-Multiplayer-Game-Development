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

    private ulong PlayerId = 0;

    void Start()
    {
        PlayerId = gameManager.currentPlayerId;
        gameManager = FindObjectOfType<GameManager>();
        turnIndicatorText = turnIndicator.GetComponent<Text>();
        UpdateTurnIndicator(PlayerId);
    }


    public void UpdateTurnIndicator(ulong currentPlayerID)
    {
        if (gameManager.isGameActive)
        {
            if (PlayerId == currentPlayerID)
            {
                turnIndicatorText.text = "It's your turn";
            }
            else
            {
                turnIndicatorText.text = "It is the other player's turn";
            }
        }
    }
}
