using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;

public class RoomListCache : MonoBehaviourPunCallbacks
{
    private TypedLobby customLobby = new(null, LobbyType.Default);

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    public static RoomListCache Instance;


    private void Start()
    {
        Instance = this;
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(customLobby);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        cachedRoomList.Clear();
    }

    public List<RoomInfo> GetCommonRoomList(string[] interests)
    {
        List<RoomInfo> similarRooms = new();
        List<RoomInfo> noInterestRooms = new();

        foreach (KeyValuePair<string,RoomInfo> kv in cachedRoomList)
        {
            if (kv.Value.CustomProperties.ContainsKey("interests"))
            {
                string[] interestsArray = (string[])kv.Value.CustomProperties["interests"];
                var result = interestsArray.Intersect(interests);
                if (result.Count() > 0)
                {
                    similarRooms.Add(kv.Value);
                    if (similarRooms.Count > 10)
                    {
                        break;
                    }
                }
            } else
            {
                noInterestRooms.Add(kv.Value);
            }

        }
        return similarRooms.Count > 0 ? similarRooms : noInterestRooms;
    }
}