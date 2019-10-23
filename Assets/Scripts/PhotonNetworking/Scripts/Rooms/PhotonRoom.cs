using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room Info
    public static PhotonRoom Instance;
    private PhotonView PV;

    public bool m_IsGameLoaded;
    public string m_CurrentSceneName;
    

    private Player[] photonPlayers;
    public int m_PlayersInRoom;
    // Display the number of players in room
    public int m_MyNumbersInRoom;

    public int m_PlayersInGame;

    // Delayed Start
    public static bool bDelayStart = true;
    private bool m_readyToCount;
    private bool m_readyToStart;
    // Counting down time
    public float m_StartingTime;
    private float m_lessThanMaxPlayers;
    private float m_atMaxPlayers;
    private float m_timeToStart;


    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPannel;
    public GameObject playerListingPrefab;
    public GameObject startButotn;

    public Text RoomName;

    private void Awake()
    {
        if (PhotonRoom.Instance == null)
        {
            PhotonRoom.Instance = this;
        }
        else
        {
            if (PhotonRoom.Instance != this)
            {
                Destroy(PhotonRoom.Instance.gameObject);
                PhotonRoom.Instance = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }


    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        m_readyToCount = false;
        m_readyToStart = false;

        m_lessThanMaxPlayers = m_StartingTime;
        m_atMaxPlayers = Constants.MAX_PLAYER_IN_ROOM;
        m_timeToStart = m_StartingTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Delay Start
        if (bDelayStart)
        {
            if(m_PlayersInRoom == 1)
            {
                RestartTimer();
            }
            if (!m_IsGameLoaded)
            {
                if (m_readyToStart)
                {
                    m_atMaxPlayers -= Time.deltaTime;
                    m_lessThanMaxPlayers = m_atMaxPlayers;
                    m_timeToStart = m_atMaxPlayers;
                }
                else if (m_readyToCount)
                {
                    m_lessThanMaxPlayers -= Time.deltaTime;
                    m_timeToStart = m_lessThanMaxPlayers;
                }
                Debug.Log("Displayer time to start to the players: " + m_timeToStart);
                if(m_timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    private void RestartTimer()
    {
        m_atMaxPlayers = m_StartingTime;
        m_lessThanMaxPlayers = m_StartingTime;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in room");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            startButotn.SetActive(true);
        }
        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        m_PlayersInRoom = photonPlayers.Length;
        m_MyNumbersInRoom = m_PlayersInRoom;
        UpdateRoomName();
        // Delay start
        if (bDelayStart)
        {
            Debug.Log("Display players in room out of max player possible: " + m_PlayersInRoom + " / " + Constants.MAX_PLAYER_IN_ROOM);
            if(m_PlayersInRoom > 1)
            {
                m_readyToCount = true;
            }
            if(m_PlayersInRoom == Constants.MAX_PLAYER_IN_ROOM)
            {
                m_readyToStart = true;
                if (!PhotonNetwork.IsMasterClient) return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player: " + newPlayer.NickName + " has joined the room");

        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        m_PlayersInRoom++;
        UpdateRoomName();
        if (bDelayStart)
        {
            Debug.Log("Display players in room out of max player possible: " + m_PlayersInRoom + " / " + Constants.MAX_PLAYER_IN_ROOM);
            if (m_PlayersInRoom > 1)
            {
                m_readyToCount = true;
            }
            if (m_PlayersInRoom == Constants.MAX_PLAYER_IN_ROOM)
            {
                m_readyToStart = true;
                if (!PhotonNetwork.IsMasterClient) return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log(otherPlayer.NickName + "Has left the game");
        m_PlayersInRoom--;
        UpdateRoomName();

        ClearPlayerListings();
        ListPlayers();
    }

    public  void StartGame()
    {
        // Load the multi-player scene for all players
        m_IsGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient) return;
        if (bDelayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(Constants.MAIN_SCENE_NAME);
    }
    void ClearPlayerListings()
    {
        for (int i = playersPannel.childCount - 1; i >= 0 ; i--)
        {
            Destroy(playersPannel.GetChild(i).gameObject);
        }
    }
    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPannel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = item.NickName;
            }
        }
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        m_CurrentSceneName = scene.name;
        if(m_CurrentSceneName == Constants.MAIN_SCENE_NAME)
        {
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        // Spawn player base and let base differentiate itself to different type of player
        GameObject newPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayerBase"), Vector3.zero, Quaternion.identity);
        Debug.Log("Room: MyType: " + NetPlayerSetting.Instance.MyType);
    }

    private void UpdateRoomName()
    {
        if(RoomName)
        RoomName.text = PhotonNetwork.CurrentRoom.Name + "  " + m_PlayersInRoom + " / " + Constants.MAX_PLAYER_IN_ROOM;
    }

}
