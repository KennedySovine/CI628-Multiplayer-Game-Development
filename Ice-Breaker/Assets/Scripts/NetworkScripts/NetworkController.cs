using UnityEngine;
using Unity.Netcode;

public class NetworkController : MonoBehaviour
{
    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started successfully.");
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started successfully.");
        }
        else
        {
            Debug.LogError("Failed to start client.");
        }
    }

    public void StartServer()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Server started successfully.");
        }
        else
        {
            Debug.LogError("Failed to start server.");
        }
    }
}