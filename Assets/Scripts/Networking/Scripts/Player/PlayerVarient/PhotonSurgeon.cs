using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonSurgeon : PhotonPlayerSetupBase
{

    [Header("Local Comp")]
    [SerializeField] private Camera m_MainCam;
    [SerializeField] private GameObject m_VRGO;
    [SerializeField] private Transform m_HeadTr_Local;
    [SerializeField] private Transform m_LHandTr_Local;
    [SerializeField] private Transform m_RHandTr_Local;
    // The following three transform should be sync to the remote player
    [Header("Remote Comp")]
    [SerializeField] private Transform m_HeadTr_Remote;
    [SerializeField] private Transform m_LHandTr_Remote;
    [SerializeField] private Transform m_RHandTr_Remote;
    [SerializeField] private Transform m_Camera_Remote;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void SetupAsRemotePlayer()
    {
        base.SetupAsRemotePlayer();
        Debug.Log("Surgeon setup as remote player");

        // Destroy local component
        Destroy(m_VRGO);
        Destroy(GetComponent<OVRDebugInfo>());
    }

    protected override void SetupAsLocalPlayer()
    {
        base.SetupAsLocalPlayer();
        Debug.Log("Surgeon setup as local player");

        // 1. Main Camera
        m_MainCam.tag = "MainCamera";

        // 2. Destroy Remote Component
        //     Destroy local player's Mesh and the sync camera comp
        Destroy(m_HeadTr_Remote.GetChild(0).gameObject);
        Destroy(m_LHandTr_Remote.GetChild(0).gameObject);
        Destroy(m_RHandTr_Remote.GetChild(0).gameObject);
        Destroy(m_Camera_Remote.GetComponent<Camera>());
    }

    void ConstrainTransform(ref Transform i_source, Transform i_Target)
    {
        i_source.position = i_Target.position;
        i_source.rotation = i_Target.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {

            if(m_HeadTr_Local && m_LHandTr_Local && m_RHandTr_Local && m_MainCam)
            {
                // Network sync
                ConstrainTransform(ref m_HeadTr_Remote, m_HeadTr_Local);
                ConstrainTransform(ref m_LHandTr_Remote, m_LHandTr_Local);
                ConstrainTransform(ref m_RHandTr_Remote, m_RHandTr_Local);
                ConstrainTransform(ref m_Camera_Remote, m_MainCam.transform);
            }
        }
    }
}
