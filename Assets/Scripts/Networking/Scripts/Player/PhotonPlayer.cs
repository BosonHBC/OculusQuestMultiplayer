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
        transform.position = playerStart.position;
        transform.rotation = playerStart.rotation;
        transform.name = prafabName + "_parent_" + (PV.IsMine ? "IsMine" : "NotMine");
        // Set up avatar transform
        myAvatar = Instantiate(Resources.Load<GameObject>(Path.Combine("PhotonPrefabs", prafabName)), Vector3.zero, Quaternion.identity);
        myAvatar.name = prafabName;
        myAvatar.transform.parent = transform;
        myAvatar.transform.localPosition = Vector3.zero;
        myAvatar.transform.localRotation = Quaternion.identity;
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
