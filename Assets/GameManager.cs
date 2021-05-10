using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject sceneCamera;

    private void Awake()
    {
        gameCanvas.SetActive(true);
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        gameCanvas.SetActive(false);
        //sceneCamera.SetActive(false);
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

}
