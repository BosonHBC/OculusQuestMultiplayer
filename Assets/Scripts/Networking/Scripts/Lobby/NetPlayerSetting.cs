using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Surgeon,
    RemoteOP,
    Nurse

}

public class NetPlayerSetting : MonoBehaviour
{
    public static NetPlayerSetting Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_PlayerTypeNPrefabNameMap.Clear();
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.Surgeon, "0_Surgeon");
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.RemoteOP, "1_RemoteOperator");
            m_PlayerTypeNPrefabNameMap.Add(PlayerType.Nurse, "2_Nurse");
        }
    }

    private Dictionary<PlayerType, string> m_PlayerTypeNPrefabNameMap = new Dictionary<PlayerType, string>();
    public Dictionary<PlayerType, string> Type2PrefabName { get => m_PlayerTypeNPrefabNameMap;}

    private PlayerType m_myType;
    public PlayerType MyType { get => m_myType; set => m_myType = value; }
}
