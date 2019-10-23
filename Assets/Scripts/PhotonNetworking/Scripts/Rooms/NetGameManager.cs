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
            PV = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
