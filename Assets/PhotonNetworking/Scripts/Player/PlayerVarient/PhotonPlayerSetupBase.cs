using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerSetupBase : MonoBehaviour
{
    // static functions
   public static  void ConstrainTransform(ref Transform i_source, Transform i_Target)
    {
        i_source.position = i_Target.position;
        i_source.rotation = i_Target.rotation;
    }

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

    // Start is called before the first frame update
    
        public void SetUpReference(PhotonView i_PV, PhotonPlayer i_PP)
    {
        PV = i_PV;
        m_myType = i_PP.myType;

        if (PV.IsMine)
        {
            SetupAsLocalPlayer();
            NetPlayerSetting.Instance.MyType = m_myType;
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

    public virtual void StartSimulation()
    {
        // Set up local behaviours
        Constants.Log(name + " starts simulation!");
    }
}
