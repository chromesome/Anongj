using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.WhyJam.Anongj
{
    public class MenuController : MonoBehaviourPunCallbacks
    {
        #region Singleton
        public static MenuController Instance;

        private void Awake()
        {
            Instance = this;
        }
        #endregion

        [SerializeField] private byte maxPlayers = 5;

        [SerializeField] private GameObject usernameMenu;
        [SerializeField] private GameObject controlPanel;
    
        [SerializeField] private InputField usernameInput;
        [SerializeField] private InputField createGameInput;
        [SerializeField] private InputField joinGameInput;

        [SerializeField] private GameObject startButton;

        [SerializeField] private RoomList roomContentList;

        //private void Awake()
        //{
        //    PhotonNetwork.ConnectUsingSettings();
        //}

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            usernameMenu.SetActive(true);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected to game server");
            //base.OnConnectedToMaster();
        }

        public void OnChangeUserNameInput()
        {
            if(usernameInput.text.Length > 3)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
        }

        public void SetUserName()
        {
            usernameMenu.SetActive(false);
            PhotonNetwork.NickName = usernameInput.text;
        }

        public void CreateGame()
        {
            PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { MaxPlayers = maxPlayers }, null);
        }

        public void JoinGame()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
        }

        public void RefreshRoomList()
        {
            // TODO esto parece que no funciona, mejor manejarse solo con contraseña corte amongus?
            PhotonNetwork.GetCustomRoomList(TypedLobby.Default, "");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            roomContentList.AddList(roomList);
        }

        public void SelectGameRoom(RoomButton roomButton)
        {
            joinGameInput.text = roomButton.getRoomName();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            //PhotonNetwork.LoadLevel("MainGame");
            PhotonNetwork.LoadLevel("MPTest");
        }
    }

}
