using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject tileParent;
    private GameObject[] tileObjects;

    private Rigidbody[] tileRigidbodies;
    private GameObject highlightedTile;

    void Start()
    {
        gameManager = GameManager.Instance;
        tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        tileRigidbodies = new Rigidbody[tileObjects.Length];

        for (int i = 0; i < tileObjects.Length; i++)
        {
            tileRigidbodies[i] = tileObjects[i].GetComponent<Rigidbody>();
            tileRigidbodies[i].useGravity = false; // Gravity off initially
        }
    }

    void Update()
    {
        HandleTileHighlight();
    }

    private void HandleTileHighlight()
    {
        if (!gameManager) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject tileUnderMouse = hit.collider.gameObject;

            if (tileUnderMouse.CompareTag("Tile") && tileUnderMouse != highlightedTile)
            {
                if (highlightedTile != null)
                {
                    SetTileHighlight(highlightedTile, false);
                }

                highlightedTile = tileUnderMouse;
                SetTileHighlight(highlightedTile, true);
            }
        }
        else if (highlightedTile != null)
        {
            SetTileHighlight(highlightedTile, false);
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

    public void HandleTileClick(GameObject tile)
    {
        Tile tileComponent = tile.GetComponent<Tile>();
        if (highlightedTile == tile && !tileComponent.IsOccupied)
        {
            tileComponent.IsOccupied = true;
            TriggerGravity();
        }
    }

    private void TriggerGravity()
    {
        foreach (Rigidbody rb in tileRigidbodies)
        {
            rb.useGravity = true;
        }

        StartCoroutine(CheckTileMotionCoroutine());
    }

    private IEnumerator CheckTileMotionCoroutine()
    {
        while (true)
        {
            bool anyTileMoving = false;

            foreach (Rigidbody rb in tileRigidbodies)
            {
                if (rb.velocity.magnitude > 0.1f) // Tile is still moving
                {
                    anyTileMoving = true;
                    break;
                }
            }

            if (!anyTileMoving)
            {
                DisableGravity();
                gameManager.StartNextTurn();
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void DisableGravity()
    {
        foreach (Rigidbody rb in tileRigidbodies)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
}
