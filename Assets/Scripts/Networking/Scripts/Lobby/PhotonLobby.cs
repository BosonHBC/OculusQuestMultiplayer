using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby Instance;

    public string m_RoomName = "Server";
    public int m_RoomSize;
    [SerializeField] private GameObject m_roomListPrefab;
    [SerializeField] private Transform m_roomsPannel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the photon master server");
        //base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player_" + Random.Range(0,1000);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();
        foreach (var room in roomList)
        {
            ListRoom(room);
        }
    }


    private void RemoveRoomListings()
    {
        while(m_roomsPannel.childCount != 0)
        {
            Destroy(m_roomsPannel.GetChild(0).gameObject);
        }
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempRoomList = Instantiate(m_roomListPrefab, m_roomsPannel);

            RoomBotton tempButton = tempRoomList.GetComponent<RoomBotton>();
            tempButton.SetRoom(room.Name, room.MaxPlayers);
        }
    }


    public void CreateRoom()
    {
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)Constants.MAX_PLAYER_IN_ROOM };
        PhotonNetwork.CreateRoom(m_RoomName, roomOps);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnRoomNameChange(string i_name)
    {
        m_RoomName = i_name;
    }


    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
