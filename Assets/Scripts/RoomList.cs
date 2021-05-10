using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Com.WhyJam.Anongj
{
    public class RoomList : MonoBehaviour
    {
        [SerializeField] private RoomButton prefab;

        private List<RoomButton> rooms;

        private void Awake()
        {
            rooms = new List<RoomButton>();
        }

        public void AddList(List<RoomInfo> roomList)
        {
            if(roomList.Count > 0)
            {
                

                foreach (RoomInfo room in roomList)
                {
                    if (room.RemovedFromList)
                    {
                        int index = rooms.FindIndex(x => x.getRoomName() == room.Name);

                        if(index != -1)
                        {
                            Destroy(rooms[index].gameObject);
                            rooms.RemoveAt(index);
                        }
                    }
                    else
                    {
                        RoomButton newObj;

                        int index = rooms.FindIndex(x => x.getRoomName() == room.Name);

                        if(index != -1)
                        {
                            newObj = rooms[index];
                        }
                        else
                        {
                            newObj = Instantiate(prefab, transform);
                            rooms.Add(newObj);
                        }

                        RoomButton button = newObj.GetComponent<RoomButton>();

                        if(button)
                        {
                            button.setRoomInfo(room);
                        }
                    }
                }
            }
        }
    }
}