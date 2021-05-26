using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // Match config
    public int kingMistakes = 5;
    public int anomiesAlive = -1;

    [SerializeField] GameObject kingPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject sceneCamera;

    [SerializeField] Text roomNameText;
    [SerializeField] Text whoIsKingText;
    [SerializeField] Text pingText;
    [SerializeField] Button startButton;

    // In game action feed
    [SerializeField] GameObject playerFeed;
    [SerializeField] GameObject feedGrid;

    [SerializeField] bool debugKing = false;

    // Hacer que se vuelva a abrir el menu en lugar de duplicar el boton
    [SerializeField] GameObject leaveRoom2;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = _instance = GameObject.FindObjectOfType<GameManager>();
        }


        gameCanvas.SetActive(true);
        if(PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            roomNameText.text += PhotonNetwork.CurrentRoom.Name;
            whoIsKingText.text += PhotonNetwork.MasterClient.NickName;
            if(PhotonNetwork.IsMasterClient)
            {
                startButton.enabled = true;
            }
            else
            {
                startButton.enabled = false; // Wait for king to start
                Text buttonText = startButton.GetComponentInChildren<Text>();
                if (buttonText)
                {
                    buttonText.text = "Espera al rey";
                }
            }
        }
        else
        {
            roomNameText.text += "NO ROOM";
            whoIsKingText.text += debugKing ? "I'M KING" : "NO KING";
        }
    }

    private void Start()
    {
        AnonNPC.OnKilledNPC += OnNPCKilled;
        AnonController.OnKillPlayer += OnPlayerKilled;
    }

    public void OnNPCKilled()
    {
        kingMistakes--;
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Haz matado une inocente!");
            PrintPlayerFeedMessage("Haz matado a alguien inocente!");

            if (kingMistakes == 0)
            {
                Debug.Log("El pueblo está arto de tu reinado, perdiste!");
                PrintPlayerFeedMessage("El pueblo está arto de tu reinado, perdiste!");
            }
            else
            {
                Debug.Log(string.Format("Te quedan {0} intentos", kingMistakes));
                PrintPlayerFeedMessage(string.Format("Tus subdites no estan felices con tus acciones. Te quedan {0} intentos", kingMistakes));
            }
        }
        else
        {
            Debug.Log("Haz matado une inocente!");
            PrintPlayerFeedMessage("El rey a asesinado une anomie inocente!");

            if (kingMistakes == 0)
            {
                Debug.Log("Estamos hartos de los caprichos del rey, el pueblo tome acción!");
                PrintPlayerFeedMessage("Estamos artos de los caprichos del rey, el pueblo tome acción!");
                // TODO Fin del juego gana el pueblo
            }
            else
            {
                Debug.Log(string.Format("Te quedan {0} intentos", kingMistakes));
                PrintPlayerFeedMessage(string.Format("Pero está bien, le lo dejaremos pasar {0} veces más", kingMistakes));
            }
        }
    }

    public void OnPlayerKilled(string gamerTag)
    {
        anomiesAlive--;
        Debug.Log("Haz matado a " + gamerTag);
        if (gamerTag.Equals(PhotonNetwork.NickName))
        {
            PrintPlayerFeedMessage(string.Format("El rey te ha asesinado, descansa en paz {0}", gamerTag));
            // TODO Pantalla de muerte de anomie
            // Puede seguir dando vueltas como fantasma y opción de salir de la partida?
            // Leyenda "Haz muerto" y mostrar la camara central y opción de salir.
            // Respawnear?
        }
        else
        {
            PrintPlayerFeedMessage(string.Format("Que bien que bien, el rey ah aniquilado a {0} por nuestro bien! ¿no?", gamerTag));
        }

        if (anomiesAlive == 0)
        {
            PrintPlayerFeedMessage("El rey ha limpiado el pueblo de disconformistas, todes somos felices!");
            // TODO Fin del juego gana el rey
        }
    }    

    private void Update()
    {
        //pingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void StartGame()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if(photonView)
            {
                photonView.RPC("SpawnPlayer", RpcTarget.All);
            }

            anomiesAlive = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        }
        else
        {
            SpawnPlayer();
        }
    }

    [PunRPC]
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

    private void OnDisable()
    {
        AnonNPC.OnKilledNPC -= OnNPCKilled;
        AnonController.OnKillPlayer -= OnPlayerKilled;
    }

    public void PrintPlayerFeedMessage(string formatedMessage)
    {
        GameObject feed = Instantiate(playerFeed, Vector2.zero, Quaternion.identity);
        feed.transform.SetParent(feedGrid.transform, false);
        feed.GetComponent<Text>().text = formatedMessage;
    }
}
