using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<int, PlayerData> playerEntities = new Dictionary<int, PlayerData>();
    private int currentPlayerIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(PlayerData player)
    {
        int playerId = playerEntities.Count;
        playerEntities.Add(playerId, player);

        if (playerId == 0)
        {
            StartNextTurn(); // Start with the first player's turn
        }
    }

    public void StartNextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % playerEntities.Count;

        foreach (var player in playerEntities.Values)
        {
            player.IsTurn = false; // Disable all players
        }

        playerEntities[currentPlayerIndex].IsTurn = true; // Enable the next player
    }
}
