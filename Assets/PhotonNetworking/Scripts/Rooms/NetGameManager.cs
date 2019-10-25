using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ---------
// This class handles the simulation loop, mainly control game-start, passing remote operator parameters
// ----------

public class NetGameManager : MonoBehaviour
{
    #region INSTANCE
    public static NetGameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            name = "GameManager";
            PV = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private PhotonView PV;

    bool m_bGameStart;
    public void StartSimulation()
    {
        // Only Remote operator can start game.
        if(!m_bGameStart )
        {
            if(NetPlayerSetting.Instance.MyType == PlayerType.RemoteOP)
            {
                PV.RPC("RPC_StartSimulation", RpcTarget.AllBuffered);
            }
            else
            {
                GameplayStatics.Log("Start Game Fail because wrong player type, myType: " + NetPlayerSetting.Instance.MyType);
            }

        }
        else
        {
            GameplayStatics.Log("Start Game Fail because game has already started");
        }
    }

    [PunRPC]
    void RPC_StartSimulation()
    {
        m_bGameStart = true;
        // Data manager set up data

        // Call start simulation function of all joined players
        foreach (PhotonPlayerSetupBase item in NetworkReferences.Instance.m_PlayerList)
        {
            item.StartSimulation();
        }
    }
}
