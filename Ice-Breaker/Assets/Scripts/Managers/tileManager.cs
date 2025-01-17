using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject tileParent;
    private GameObject[] tileObjects;

    private Rigidbody[] tileRigidbodies; // Track all 3D Rigidbody components
    private GameObject highlightedTile;  // Current highlighted tile

    void Start()
    {
        gameManager = GameManager.Instance;
        tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        tileRigidbodies = new Rigidbody[tileObjects.Length];

        for (int i = 0; i < tileObjects.Length; i++)
        {
            tileRigidbodies[i] = tileObjects[i].GetComponent<Rigidbody>();
            tileRigidbodies[i].useGravity = false; // Turn off gravity initially
        }
    }

    void Update()
    {
        HandleTileHighlight();
    }

    // Highlight the tile under the mouse cursor
    private void HandleTileHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject tileUnderMouse = hit.collider.gameObject;

            if (tileUnderMouse.CompareTag("Tile") && tileUnderMouse != highlightedTile)
            {
                if (highlightedTile != null)
                {
                    SetTileHighlight(highlightedTile, false); // Turn off highlight on the previous tile
                }

                highlightedTile = tileUnderMouse;
                SetTileHighlight(highlightedTile, true); // Highlight the new tile
            }
        }
        else if (highlightedTile != null)
        {
            SetTileHighlight(highlightedTile, false); // Turn off highlight if no tile is hit
            highlightedTile = null;
        }
    }

    private void SetTileHighlight(GameObject tile, bool highlight)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = highlight ? Color.yellow : Color.white;
        }
    }

    // Trigger gravity when a valid tile is clicked
    public void TriggerGravity()
    {
        foreach (Rigidbody rb in tileRigidbodies)
        {
            rb.useGravity = true; // Enable gravity
        }

        StartCoroutine(CheckTileMotionCoroutine());
    }

    // Check when tiles stop moving
    private IEnumerator CheckTileMotionCoroutine()
    {
        while (true)
        {
            bool anyTileMoving = false;

            foreach (Rigidbody rb in tileRigidbodies)
            {
                if (rb.velocity.magnitude > 0.1f) // If any tile is moving
                {
                    anyTileMoving = true;
                    break;
                }
            }

            if (!anyTileMoving)
            {
                DisableGravity();
                gameManager.StartNextTurn(); // Notify GameManager to start the next player's turn
                yield break; // Stop checking
            }

            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    private void DisableGravity()
    {
        foreach (Rigidbody rb in tileRigidbodies)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero; // Stop motion
        }
    }

    public void HandleTileClick(GameObject tile)
    {
        if (highlightedTile == tile && !tile.GetComponent<Tile>().IsOccupied)
        {
            tile.GetComponent<Tile>().IsOccupied = true; // Mark the tile as occupied
            TriggerGravity(); // Start gravity
        }
    }
}