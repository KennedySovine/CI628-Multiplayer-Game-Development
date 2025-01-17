using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    private Dictionary<ulong, PlayerData> playerEntities = new Dictionary<ulong, PlayerData>();
    private ulong currentPlayerId;
    private bool isGameActive = false;

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

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Listen for player connections
            NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnPlayerDisconnected;
        }
    }

    private void OnPlayerConnected(ulong clientId)
    {
        Debug.Log($"Player {clientId} connected.");
        if (IsServer)
        {
            // Initialize PlayerData for the newly connected client
            PlayerData newPlayer = new PlayerData { IsTurn = false };
            playerEntities.Add(clientId, newPlayer);

            // Start the game once enough players are connected
            if (playerEntities.Count >= 2 && !isGameActive)
            {
                isGameActive = true;
                StartNextTurn();
            }
        }
    }

    private void OnPlayerDisconnected(ulong clientId)
    {
        if (playerEntities.ContainsKey(clientId))
        {
            playerEntities.Remove(clientId);
            Debug.Log($"Player {clientId} disconnected.");
        }
    }

    public void StartNextTurn()
    {
        if (playerEntities.Count == 0) return;

        // Cycle to the next player
        currentPlayerId = GetNextPlayerId(currentPlayerId);

        foreach (var player in playerEntities)
        {
            player.Value.IsTurn = player.Key == currentPlayerId;
        }

        Debug.Log($"It's now Player {currentPlayerId}'s turn.");
    }

    private ulong GetNextPlayerId(ulong currentId)
    {
        var keys = new List<ulong>(playerEntities.Keys);
        int index = keys.IndexOf(currentId);
        return keys[(index + 1) % keys.Count];
    }
}