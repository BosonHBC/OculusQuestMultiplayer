using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonRemoteOP : PhotonPlayerSetupBase
{
    [Header("Local Comp")]
    [SerializeField] private GameObject m_CanvasObj;
    [SerializeField] private Camera m_Camera;

    [SerializeField] private GameObject m_startButton;
    private bool m_bCanStartGame = true;

    protected override void SetupAsRemotePlayer()
    {
        base.SetupAsRemotePlayer();

        GameplayStatics.Log("RemoteOP setup as remote player, my Type: " + NetPlayerSetting.Instance.MyType);
        // Destroy local component
        Destroy(m_Camera.gameObject);
        Destroy(m_CanvasObj);
    }

    protected override void SetupAsLocalPlayer()
    {
        base.SetupAsLocalPlayer();
        GameplayStatics.Log("RemoteOP setup as local player, my Type: " + NetPlayerSetting.Instance.MyType);

        // Initialize remote operator parameters
        m_Camera.tag = "MainCamera";
        Camera.SetupCurrent(m_Camera);

    }

    public void StartGame()
    {
        if (m_bCanStartGame)
        {
            NetGameManager.instance.StartSimulation();
        }

    }

    private void Update()
    {

    }

    public override void StartSimulation()
    {
        base.StartSimulation();
        if (PV.IsMine)
        {
            m_startButton.SetActive(false);
        }

    }
}
