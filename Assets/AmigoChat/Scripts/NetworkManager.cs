using Photon.Pun;
using UnityEngine;

namespace AmigoChat.Scripts
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private string _gameVersion = "0.1";

        public void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;

        }

        void Update()
        {

        }

        public override void OnConnectedToMaster()
        {
            RoomListCache.Instance.JoinLobby();
        }
    }
}
