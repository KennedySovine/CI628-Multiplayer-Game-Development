using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager _instance;

    public bool isGameActive;

    //[Header()]

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
}
