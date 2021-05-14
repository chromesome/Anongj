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
        roomNameText.text += PhotonNetwork.CurrentRoom.Name;
    }

    private void Update()
    {
        pingText.text = "Ping: " + PhotonNetwork.GetPing();
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
