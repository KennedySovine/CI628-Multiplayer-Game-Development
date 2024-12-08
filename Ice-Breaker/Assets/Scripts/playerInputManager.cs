using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public PlayerController playerController;

    void Awake()
    {
        if (playerPrefab != null)
        {
            playerController = playerPrefab.GetComponent<PlayerController>();
        }
    }
    void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        Debug.Log("Move: " + move);
    }
}
