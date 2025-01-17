using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<ulong, PlayerData> playerEntities = new Dictionary<ulong, PlayerData>();
    public ulong currentPlayerId;
    public bool isGameActive = false;
    public NetworkManager networkManager;

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

        // Register network callbacks (if running as the server or host)
        if (!networkManager.IsClient)
        {
            networkManager.OnClientConnectedCallback += OnPlayerConnected;
            networkManager.OnClientDisconnectCallback += OnPlayerDisconnected;
        }
        Time.timeScale = 0; // Pause the game until at least two players are connected
    }

    private void OnDestroy()
    {
        // Unregister network callbacks
        if (networkManager != null && !networkManager.IsClient)
        {
            networkManager.OnClientConnectedCallback -= OnPlayerConnected;
            networkManager.OnClientDisconnectCallback -= OnPlayerDisconnected;
        }
    }

    private void Update()
    {
        Debug.Log(playerEntities.Count + " players connected.");
    }

    private void OnPlayerConnected(ulong clientId)
    {
        Debug.Log($"Player {clientId} connected.");

        if (networkManager.IsServer || networkManager.IsHost)
        {
            // Initialize player data for the newly connected client
            PlayerData newPlayer = new PlayerData { IsTurn = false };
            playerEntities.Add(clientId, newPlayer);
            Debug.Log($"Player {clientId} added to the game. Total players: {playerEntities.Count}");

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
            Debug.Log($"Player {clientId} disconnected. Total players: {playerEntities.Count}");
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
        FindObjectOfType<UIManager>().UpdateTurnIndicator(currentPlayerId); // Notify UIManager
    }

    private ulong GetNextPlayerId(ulong currentId)
    {
    List<ulong> playerIds = new List<ulong>(playerEntities.Keys);
    int currentIndex = playerIds.IndexOf(currentId);

    if (currentIndex == -1 || playerIds.Count == 0)
    {
        // If the current ID is not found or there are no players, return the first player ID
        return playerIds.Count > 0 ? playerIds[0] : 0;
    }

    // Get the next player ID, wrapping around if necessary
    int nextIndex = (currentIndex + 1) % playerIds.Count;
    return playerIds[nextIndex];
    }
}
