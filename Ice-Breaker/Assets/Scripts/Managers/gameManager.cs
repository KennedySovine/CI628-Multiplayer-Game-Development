using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<ulong, PlayerData> playerEntities = new Dictionary<ulong, PlayerData>();
    public ulong currentPlayerId;
    public bool isGameActive = false;
    private NetworkManager networkManager;

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

    private void Start()
    {
        // Find the NetworkManager by name
        GameObject networkManagerObject = GameObject.Find("NetworkManager");
        if (networkManagerObject == null)
        {
            Debug.LogError("NetworkManager GameObject not found! Please ensure it exists in the scene.");
            return;
        }

        networkManager = networkManagerObject.GetComponent<NetworkManager>();
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager component not found on the NetworkManager GameObject!");
            return;
        }

        // Register network callbacks (only if running as the server)
        if (networkManager.IsServer)
        {
            networkManager.OnClientConnectedCallback += OnPlayerConnected;
            networkManager.OnClientDisconnectCallback += OnPlayerDisconnected;
        }
        Time.timeScale = 0; // Pause the game until at least two players are connected
    }

    private void OnDestroy()
    {
        // Unregister network callbacks
        if (networkManager != null && networkManager.IsServer)
        {
            networkManager.OnClientConnectedCallback -= OnPlayerConnected;
            networkManager.OnClientDisconnectCallback -= OnPlayerDisconnected;
        }
    }

    private void OnPlayerConnected(ulong clientId)
    {
        Debug.Log($"Player {clientId} connected.");

        if (networkManager.IsServer)
        {
            // Initialize player data for the newly connected client
            PlayerData newPlayer = new PlayerData { IsTurn = false };
            playerEntities.Add(clientId, newPlayer);

            // Start the game when at least two players are connected
            if (playerEntities.Count >= 2 && !isGameActive)
            {
                Time.timeScale = 1; // Resume the game
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
            Time.timeScale = 0;
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
        FindObjectOfType<UIManager>().UpdateTurnIndicator(); // Notify UIManager
        Time.timeScale = 0; //Pause game until play is made.
    }

    private ulong GetNextPlayerId(ulong currentId)
    {
        var keys = new List<ulong>(playerEntities.Keys);
        int index = keys.IndexOf(currentId);
        return keys[(index + 1) % keys.Count];
    }
}
