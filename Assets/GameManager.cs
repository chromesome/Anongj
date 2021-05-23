using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject kingPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject sceneCamera;

    [SerializeField] Text roomNameText;
    [SerializeField] Text whoIsKingText;
    [SerializeField] Text pingText;

    [SerializeField] bool debugKing = false;

    // Hacer que se vuelva a abrir el menu en lugar de duplicar el boton
    [SerializeField] GameObject leaveRoom2;

    private void Awake()
    {
        gameCanvas.SetActive(true);
        if(PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            roomNameText.text += PhotonNetwork.CurrentRoom.Name;
            whoIsKingText.text += PhotonNetwork.MasterClient.NickName;
        }
        else
        {
            roomNameText.text += "NO ROOM";
            whoIsKingText.text += debugKing ? "I'M KING" : "NO KING";
        }
    }

    private void Update()
    {
        //pingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void SpawnPlayer()
    {
        if(PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(kingPrefab.name, Vector3.zero, Quaternion.identity, 0);
                sceneCamera.SetActive(false);
            }
            else
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
                sceneCamera.SetActive(false);
            }
        }
        else
        {
            if(debugKing)
            {
                GameObject go = Instantiate(kingPrefab, Vector3.zero, Quaternion.identity);
                go.SetActive(true);
                sceneCamera.SetActive(false);
            }
            else
            {
                GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                go.SetActive(true);
                sceneCamera.SetActive(false);
            }
        }

        gameCanvas.SetActive(false);
        leaveRoom2.SetActive(true);
        //sceneCamera.SetActive(false);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

}
