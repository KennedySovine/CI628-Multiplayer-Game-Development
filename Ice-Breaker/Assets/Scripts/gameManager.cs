using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager _instance;

    public bool isGameActive;

    [Header("Game Objects")]
    private float tileRadius = 30.00001f;
    [SerializeField] private GameObject parentGroup;
    [SerializeField] private GameObject tilePrefab;
    public GameObject[] player1Tiles = new GameObject[19];
    public GameObject[] player2Tiles = new GameObject[19];

    #region Singleton

    public static gameManager instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<gameManager>();
            }
            return _instance;
        }
    }
    //Game manager is persistent across scenes
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            // Game logic goes here
        }
    }

    public void EndGame()
    {
        isGameActive = false;
    }

    public void fillBoard(){
        float xOffset = tileRadius * 1.5f;
        float yOffset = tileRadius * Mathf.Sqrt(3);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Offset odd columns to create staggered rows
                float xPos = col * xOffset;
                float yPos = row * yOffset;

                if (col % 2 == 1)
                {
                    yPos += yOffset / 2;
                }

                Vector3 tilePosition = new Vector3(xPos, yPos, 0);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.SetParent(parentGroup.transform);

                // Optional: Rename tile for clarity
                newTile.name = $"Tile_{row}_{col}";
            }
        }
    }
}
