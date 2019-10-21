using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerSetupBase : MonoBehaviour
{
    protected struct SimpleTransform
    {
       public Vector3 Pos;
        public Quaternion Quat;

        public void LerpToThisTransform(Transform i_Transform, float i_LerpTime)
        {
            i_Transform.position = Vector3.Lerp(i_Transform.position, Pos, i_LerpTime);
            i_Transform.rotation = Quaternion.Lerp(i_Transform.rotation, Quat, i_LerpTime);
        }
    }

    [SerializeField] protected PhotonView PV;
    public PlayerType m_myType;

    [SerializeField]
    protected float m_SendRate = 0.05f;
    protected float m_sendCollpaseTime;
    // Start is called before the first frame update
    
        public void SetUpReference(PhotonView i_PV, PhotonPlayer i_PP)
    {
        PV = i_PV;
        m_myType = i_PP.myType;
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
