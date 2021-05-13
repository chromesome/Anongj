using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private PlayerListRow playerListPrefab;

    private List<PlayerListRow> playerList = new List<PlayerListRow>();

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    private void GetCurrentRoomPlayers()
    {
        if(PhotonNetwork.IsConnected)
        {
            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerRow(playerInfo.Value);
            }
        }
    }

    private void AddPlayerRow(Player player)
    {
        PlayerListRow row = Instantiate(playerListPrefab, content);
        if (row != null)
        {
            row.SetPlayerInfo(player);
            playerList.Add(row);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        AddPlayerRow(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerList.FindIndex(x => x.Player == otherPlayer);

        if(index != -1)
        {
            Destroy(playerList[index].gameObject);
            playerList.RemoveAt(index);
        }
    }
}
