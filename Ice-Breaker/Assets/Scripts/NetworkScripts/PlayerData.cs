using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool IsTurn { get; set; }

    private tileManager tileManager;

    private void Start()
    {
        tileManager = FindObjectOfType<tileManager>();
        GameManager.Instance.RegisterPlayer(this);
    }

    private void Update()
    {
        if (IsTurn)
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
