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
    [SerializeField] private Transform m_LHandTr_Remote;
    [SerializeField] private Transform m_RHandTr_Remote;
    [SerializeField] private Transform m_Camera_Remote;

    // private received transformation data
    private SimpleTransform m_LHand_NetRef;
    private SimpleTransform m_RHand_NetRef;
    private SimpleTransform m_Camera_NetRef;
    private float m_LerpTime;
    // Start is called before the first frame update
    protected void Start()
    {
        m_LerpTime = m_SendRate;
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
        Destroy(m_LHandTr_Remote.GetChild(0).gameObject);
        Destroy(m_RHandTr_Remote.GetChild(0).gameObject);
        Destroy(m_Camera_Remote.GetChild(0).gameObject);
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
            // if it is local player, update network reference for remote clients
            if (m_HeadTr_Local && m_LHandTr_Local && m_RHandTr_Local && m_MainCam)
            {
                // prepare data for Network sync
                ConstrainTransform(ref m_LHandTr_Remote, m_LHandTr_Local);
                ConstrainTransform(ref m_RHandTr_Remote, m_RHandTr_Local);
                ConstrainTransform(ref m_Camera_Remote, m_MainCam.transform);
            }
            //HandleSendTransformation();
        }
        else
        {
            // if it is a remote client, Lerp the movement according to network reference
         //   m_LHand_NetRef.LerpToThisTransform(m_LHandTr_Remote, m_LerpTime);
         //   m_RHand_NetRef.LerpToThisTransform(m_RHandTr_Remote, m_LerpTime);
         //   m_Camera_NetRef.LerpToThisTransform(m_Camera_Remote, m_LerpTime);
        }
    }

    void HandleSendTransformation()
    {
        m_sendCollpaseTime += Time.deltaTime;
        if (m_sendCollpaseTime > m_SendRate)
        {
            m_sendCollpaseTime = 0;
            // Send one data to all other client of my type
              PV.RPC("RPC_ReceiveTransforms", RpcTarget.Others, 
                  m_LHandTr_Remote.position, m_LHandTr_Remote.rotation, 
                  m_RHandTr_Remote.position, m_RHandTr_Remote.rotation, 
                  m_Camera_Remote.position, m_Camera_Remote.rotation);
        }
    }

    [PunRPC]
    void RPC_ReceiveTransforms(
        Vector3 i_LHand_Pos, Quaternion i_LHand_Quta,
        Vector3 i_RHand_Pos, Quaternion i_RHand_Quta,
        Vector3 i_Camera_Pos, Quaternion i_Camera_Quta)
    {
        m_LHand_NetRef.Pos = i_LHand_Pos;
        m_LHand_NetRef.Quat = i_LHand_Quta;

        m_RHand_NetRef.Pos =    i_RHand_Pos;
        m_RHand_NetRef.Quat =   i_RHand_Quta;

        m_Camera_NetRef.Pos = i_Camera_Pos;
        m_Camera_NetRef.Quat = i_Camera_Quta;
    }
}