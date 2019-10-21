using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    public PlayerType myType = PlayerType.None;
    [SerializeField] private GameObject myAvatar;

    private bool m_bMyTypeReceived = false;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            // Local player, send my type to everyone
            myType = NetPlayerSetting.Instance.MyType;
            PV.RPC("RPC_ReceiveType", RpcTarget.AllBuffered, (int)myType);
        }
    }

    private void SetUpAvatarAndTransformAccordingToType()
    {
        string prafabName = "PhotonNetworkPlayerTest";
        // Check If it contains this player type
        {

            if (!NetPlayerSetting.Instance.Type2PrefabName.ContainsKey(myType))
            {
                Debug.LogError("Fail to find relevant type [" + myType + "] in prefab folders, fail to create VR Player");
            }
            else
            {
                // Set proper name
                prafabName = NetPlayerSetting.Instance.Type2PrefabName[myType];
            }
        }
        // Set up Player base transform
        Transform playerStart = NetworkPositions.Instance.m_PlayerStartPositions[(int)myType];

        if (PV.IsMine)
        {
            // Set up avatar transform
            myAvatar = PhotonNetwork.Instantiate(/*Resources.Load<GameObject>*/(Path.Combine("PhotonPrefabs", prafabName)), playerStart.position, playerStart.rotation);
            PV.RPC("SetUpLocalAvatar", RpcTarget.AllBuffered, myAvatar.GetComponent<PhotonView>().ViewID, prafabName);
        }
    }
    [PunRPC]
    void SetUpLocalAvatar(int i_viewID, string i_name)
    {
        myAvatar = PhotonView.Find(i_viewID).gameObject;
        myAvatar.name = i_name + "_"+ (PV.IsMine ? "IsMine" : "NotMine");
        myAvatar.GetComponent<PhotonPlayerSetupBase>().SetUpReference(PV, this);
        transform.name = i_name + "_parent_" + (PV.IsMine ? "IsMine" : "NotMine");
    }

    private void Update()
    {
        if (myAvatar == null && m_bMyTypeReceived)
        {
            // Set up avatar and transform
            SetUpAvatarAndTransformAccordingToType();
        }
    }
    [PunRPC]
    void RPC_SendType(int i_myType)
    {
        PV.RPC("RPC_ReceiveType", RpcTarget.OthersBuffered, (int)i_myType);

        Debug.Log("Send My Type: " + i_myType);
    }
    [PunRPC]
    void RPC_ReceiveType(int i_myType)
    {
        myType = (PlayerType)i_myType;
        m_bMyTypeReceived = true;
        Debug.Log("Received My Type: " + myType);
    }

}
