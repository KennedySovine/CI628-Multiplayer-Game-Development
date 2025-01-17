using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject tileParent;
    public GameObject[] tileObjects;

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
            tileRigidbodies[i].isKinematic = true; // Kinematic off initially
        }

        //disableMesh();
    }

    void Update()
    {
        HandleTileHighlight();
    }

    private void HandleTileHighlight()
    {
        GameObject startMenu = GameObject.Find("StartMenu");
        if (startMenu != null && startMenu.activeSelf) return;
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
            enableMesh();
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
        yield return new WaitForSeconds(5f);
        DisableGravity();
        disableMesh();
        gameManager.StartNextTurn();
    }

    private void DisableGravity()
    {
        foreach (Rigidbody rb in tileRigidbodies)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void enableMesh(){
        foreach (GameObject tileObject in tileObjects){
            tileObject.GetComponent<MeshCollider>().enabled = true;
        }
    }

    private void disableMesh(){
        foreach (GameObject tileObject in tileObjects){
            tileObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
