using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject sceneCamera;

    [SerializeField] Text roomNameText;
    [SerializeField] Text pingText;

    private void Awake()
    {
        gameCanvas.SetActive(true);
        if(PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            roomNameText.text += PhotonNetwork.CurrentRoom.Name;    
        }
        else
        {
            roomNameText.text += "NO ROOM";
        }
    }

    private void Update()
    {
        pingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void SpawnPlayer()
    {
        if(PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);

        }
        else
        {
            GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            go.SetActive(true);
        }

        gameCanvas.SetActive(false);
        //sceneCamera.SetActive(false);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

}
