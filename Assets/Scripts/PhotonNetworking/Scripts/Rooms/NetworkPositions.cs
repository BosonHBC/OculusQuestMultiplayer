using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPositions : MonoBehaviour
{
    public static NetworkPositions Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }

    public List<Transform> m_PlayerStartPositions = new List<Transform>();
}
