using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace AmigoChat.Scripts
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public static RoomManager Instance;

        private void Start()
        {
            Instance = this;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //PhotonNetwork.CreateRoom(null,new RoomOptions { MaxPlayers = 2 });
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                ChatManager.Instance.ReportUser("Waiting For Amigo!");
            } else
            {
                ChatManager.Instance.ReportUser("Amigo Joined!");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            ChatManager.Instance.ReportUser("Amigo Joined!");
        }

        public void JoinOrCreateRoom(string interests = null)
        {
            string[] interestsArray = null;
            string[] interestProperty = {"interests"};
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
            RoomOptions newRoomOptions = new();
            newRoomOptions.MaxPlayers = 2;
            if (interests != null)
            {
                interests.ToLower();
                interestsArray = interests.Split(",");
                customRoomProperties.Add("interests",interestsArray);
                newRoomOptions.CustomRoomPropertiesForLobby = interestProperty;
                newRoomOptions.CustomRoomProperties =  customRoomProperties;
            }
            List<RoomInfo> roomsToJoin = RoomListCache.Instance.GetCommonRoomList(interestsArray);

            Debug.Log($"Sid Room Count {roomsToJoin.Count}");

            if (roomsToJoin.Count > 0)
            {
                PhotonNetwork.JoinRoom(roomsToJoin[Random.Range(0,roomsToJoin.Count)].Name);
            } else
            {
                PhotonNetwork.CreateRoom(null, newRoomOptions);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            ChatManager.Instance.ReportUser("Amigo Diconnected! Finding New Amigo");
        }
    }
}

