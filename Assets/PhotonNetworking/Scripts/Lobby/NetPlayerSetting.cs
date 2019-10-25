using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Surgeon,
    RemoteOP,
    Nurse,
    None
}


public class NetPlayerSetting : MonoBehaviour
{
    [SerializeField]
    private string[] m_PlayerPrefabName;
    public static NetPlayerSetting Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_PlayerTypeNPrefabNameMap.Clear();
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.Surgeon, m_PlayerPrefabName[0] );
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.RemoteOP, m_PlayerPrefabName[1]);
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.Nurse, m_PlayerPrefabName[2]);
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.None, "None");
        }
    }

    private Dictionary<PlayerType, string> m_PlayerTypeNPrefabNameMap = new Dictionary<PlayerType, string>();
    public Dictionary<PlayerType, string> Type2PrefabName { get => m_PlayerTypeNPrefabNameMap;}

    private PlayerType m_myType = PlayerType.None;
    public PlayerType MyType { get => m_myType; set => m_myType = value; }
}
