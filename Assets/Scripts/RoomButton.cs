using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.WhyJam.Anongj
{
    public class RoomButton : MonoBehaviour
    {
        [SerializeField] private Text buttonText;
    
        private RoomInfo roomInfo;


        public void setRoomInfo(RoomInfo room)
        {
            roomInfo = room;
            buttonText.text = string.Format("{0} ({1}/{2})", room.Name, room.PlayerCount, room.MaxPlayers);
        }

        public string getRoomName()
        {
            if (roomInfo != null)
            {
                return roomInfo.Name;
            }
            else
            {
                return "";
            }
        }

        public void JoinRoom()
        {

            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
    }
}
