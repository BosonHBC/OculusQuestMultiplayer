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

    [Header("ToggleGODependsOnHMDOrNot")]
    [SerializeField] private GameObject m_ovrControllerGO;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private VRRayButton[] rayButtons;

    public List<RoomInfo> m_roomListings;

#if UNITY_EDITOR
    public bool Debug_EnableAllButton = true;
#endif
   private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (!OVRManager.isHmdPresent)
        {
            // Not using VR, destroy the VR controller, it is not a vr player
            Destroy(m_ovrControllerGO);
#if UNITY_EDITOR
            if (Debug_EnableAllButton) return;
#endif
            // Disable create lobby
            DisableButton(ref rayButtons[0]);
            // Disable join as nurse
            DisableButton(ref rayButtons[2]);
        }
        else
        {
            // It is a VR player
            Destroy(m_mainCamera);
#if UNITY_EDITOR
            if (Debug_EnableAllButton) return;
#endif
            // Disable join as remote operator
            DisableButton(ref rayButtons[1]);
        }

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
        PhotonNetwork.NickName = "Player_" + Random.Range(0, 1000);
        OnRoomNameChange("Server");
        m_roomListings = new List<RoomInfo>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();
        int tempIndex;
        foreach (var room in roomList)
        {
            if(m_roomListings != null)
            {
                tempIndex = m_roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1)
            {
                m_roomListings.RemoveAt(tempIndex);
                Destroy(m_roomsPannel.GetChild(tempIndex).gameObject);
            }
            else
            {
                m_roomListings.Add(room);
                ListRoom(room);
            }
            
        }
    }

    static System.Predicate<RoomInfo> ByName(string Name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == Name;
        };
    }


    private void RemoveRoomListings()
    {
        int i = 0;
        while (m_roomsPannel.childCount != 0)
        {
            Destroy(m_roomsPannel.GetChild(i).gameObject);
            i++;
        }
    }

    void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempRoomList = Instantiate(m_roomListPrefab, m_roomsPannel);

            RoomBotton tempButton = tempRoomList.GetComponent<RoomBotton>();
            tempButton.SetRoom(room.Name, room.PlayerCount);
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


    public void JoinLobbyOnClick(int type)
    {
        if (!PhotonNetwork.InLobby)
        {
            // (0: is used by surgeon in creating room)
            // 1: RemoteOperator, 2: Nurse 
            NetPlayerSetting.Instance.MyType = (PlayerType)type;
            PhotonNetwork.JoinLobby();
        }
    }

    public void DisableButton(ref VRRayButton button)
    {
        button.ToggleEnableButton(false);
    }
}
