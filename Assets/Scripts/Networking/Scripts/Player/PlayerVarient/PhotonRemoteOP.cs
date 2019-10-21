using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonRemoteOP : PhotonPlayerSetupBase
{
    [Header("Local Comp")]
    [SerializeField] private GameObject m_CanvasObj;
    [SerializeField] private Camera m_Camera;

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetupAsRemotePlayer()
    {
        base.SetupAsRemotePlayer();

        Debug.Log("RemoteOP setup as remote player");
        // Destroy local component
        Destroy(m_Camera.gameObject);
        Destroy(m_CanvasObj);
    }

    protected override void SetupAsLocalPlayer()
    {
        base.SetupAsLocalPlayer();
        Debug.Log("RemoteOP setup as local player");

        // Initialize remote operator parameters
        m_Camera.tag = "MainCamera";
        Camera.SetupCurrent(m_Camera);

    }

    private void Update()
    {
        if (PV.IsMine)
        {

        }

    }
}
