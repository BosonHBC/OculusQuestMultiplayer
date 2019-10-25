using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBotton : MonoBehaviour
{
   [SerializeField] private Text NameText;
    [SerializeField] private Text SizeText;

    private string m_roomName;
    private int m_roomSize;


    public void  SetRoom(string _name, int _size)
    {
        m_roomName = _name;
        m_roomSize = _size;
        NameText.text = m_roomName;
        SizeText.text = m_roomSize.ToString() + " / " + GameplayStatics.MAX_PLAYER_IN_ROOM;
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(m_roomName);
    }
}
