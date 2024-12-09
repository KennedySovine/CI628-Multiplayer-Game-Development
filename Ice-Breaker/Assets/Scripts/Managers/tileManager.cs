using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    private gameManager gameManager;

    public GameObject[] tiles = new GameObject[19];
    [SerializeField] private GameObject tileParent;
    private GameObject[] tileObjects;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManager.Instance;
        tileObjects = GameObject.FindGameObjectsWithTag("Tile");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
