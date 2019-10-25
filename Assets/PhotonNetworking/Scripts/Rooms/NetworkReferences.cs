using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkReferences : MonoBehaviour
{
    public static NetworkReferences Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }
    public List<PhotonPlayerSetupBase> m_PlayerList = new List<PhotonPlayerSetupBase>();
    public List<Transform> m_PlayerStartPositions = new List<Transform>();
}
