using UnityEngine;
using Unity.Netcode;

public class PlayerData : NetworkBehaviour
{
    public bool IsTurn { get; set; }

    private tileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<tileManager>();

        if (tileManager == null)
        {
            Debug.LogError("tileManager not found in the scene. Please ensure a tileManager instance exists.");
            return;
        }

        if (IsOwner && IsServer)
        {
            Debug.Log($"Player {OwnerClientId} joined the game.");
        }
    }

    private void Update()
    {
        if (IsTurn && IsOwner)
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject tileUnderMouse = hit.collider.gameObject;
                if (tileUnderMouse.CompareTag("Tile"))
                {
                    tileManager.HandleTileClick(tileUnderMouse);
                }
            }
        }
    }
}
