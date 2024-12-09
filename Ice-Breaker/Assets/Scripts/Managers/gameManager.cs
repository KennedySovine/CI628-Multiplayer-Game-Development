using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    public bool isGameActive;
    public bool isTurn;

    private tileManager tileManager;

    [Header("Game Objects")]
    public GameObject[] player1Tiles = new GameObject[19];
    public GameObject[] player2Tiles = new GameObject[19];

    //Game manager is persistent across scenes
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        tileManager = GameObject.Find("Tiles").GetComponent<tileManager>();
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

}
