using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempControler : MonoBehaviour
{

    [SerializeField] Transform m_LHand;
    [SerializeField] Transform m_RHand;

    [SerializeField] Transform m_LCtrl;
    [SerializeField] Transform m_RCtrl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_LCtrl.position = m_LHand.position;
        m_LCtrl.rotation = m_LHand.rotation;
        m_RCtrl.position = m_RHand.position;
        m_RCtrl.rotation = m_RHand.rotation;
    }
}
