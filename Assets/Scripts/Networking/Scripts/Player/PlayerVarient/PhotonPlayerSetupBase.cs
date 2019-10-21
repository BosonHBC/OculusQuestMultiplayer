using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerSetupBase : MonoBehaviour
{
    [SerializeField] protected PhotonView PV;
    [SerializeField] protected PlayerType m_myType;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        PV = transform.parent.GetComponent<PhotonView>();
        m_myType = transform.parent.GetComponent<PhotonPlayer>().myType;

        if (PV.IsMine)
        {
            SetupAsLocalPlayer();
        }
        else
        {
            SetupAsRemotePlayer();
        }
    }

    // virtual function
    protected virtual void SetupAsRemotePlayer()
    {

    }
    protected virtual void SetupAsLocalPlayer()
    {

    }
}
