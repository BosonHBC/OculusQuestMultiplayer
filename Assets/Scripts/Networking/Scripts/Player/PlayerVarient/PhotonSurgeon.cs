using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonSurgeon : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    [SerializeField] private PlayerType m_myType;

    [Header("Local Comp")]
    [SerializeField] private Camera m_MainCam;
    [SerializeField] private GameObject m_VRGO;
    [SerializeField] private Transform m_HeadTr_Local;
    [SerializeField] private Transform m_LHandTr_Local;
    [SerializeField] private Transform m_RHandTr_Local;
    [Header("Remote Comp")]
    [SerializeField] private Transform m_HeadTr_Remote;
    [SerializeField] private Transform m_LHandTr_Remote;
    [SerializeField] private Transform m_RHandTr_Remote;
    // Start is called before the first frame update
    void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        m_myType = transform.parent.GetComponent<PhotonPlayer>().myType;
    }
    public void SetupAsRemotePlayer()
    {
        Destroy(m_VRGO);
    }
    public void SetupAsLocalPlayer()
    {
        // 1. Main Camera
        m_MainCam.tag = "MainCamera";

        // Destroy Remote Component
        // Destroy local player's Mesh
        Destroy(m_HeadTr_Remote.GetChild(0).gameObject);
        Destroy(m_LHandTr_Remote.GetChild(0).gameObject);
        Destroy(m_RHandTr_Remote.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
