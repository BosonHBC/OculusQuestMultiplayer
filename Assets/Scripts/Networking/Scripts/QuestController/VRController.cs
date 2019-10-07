using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour
{
    [SerializeField] private float m_MaxDist;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private Transform m_StartPoint;

    private LineRenderer lr;
    private Vector3 m_hitPoint;
    private VRRayButton m_currentButton;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPoint = transform.GetChild(0);
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastUI();
        HandleInput();
    }

    private void RaycastUI()
    {
        RaycastHit hit;
        Ray ray = new Ray(m_StartPoint.position, m_StartPoint.forward);
        UpdateCursorPosition(Vector3.zero);
        if (Physics.Raycast(ray, out hit, m_MaxDist, m_LayerMask))
        {
            // Hit canvas
            m_hitPoint = hit.point;
            UpdateCursorPosition(m_hitPoint + hit.normal * 0.01f);
            if (hit.collider.CompareTag("VRButton"))
            {
                // hover it hover the button
                if (!m_currentButton)
                {
                    m_currentButton = hit.collider.GetComponent<VRRayButton>();
                    m_currentButton.ToggleHoverButton(true);
                }
                
            }
            else
            {
                // un-hover the button
                if (m_currentButton)
                {
                    m_currentButton.ToggleHoverButton(false);
                    m_currentButton = null;
                }
            }
        }
        else
        {
            // un-hover the button
            if (m_currentButton)
            {
                m_currentButton.ToggleHoverButton(false);
                m_currentButton = null;
            }
        }

    }

    private void HandleInput()
    {
        if((OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch)
            || Input.GetKeyDown(KeyCode.A)
            ) && m_currentButton)
        {
            m_currentButton.VRClickButton();
        }
    }

    private void UpdateCursorPosition(Vector3 newLoc)
    {
        lr.SetPosition(0, newLoc);
        lr.SetPosition(1, newLoc);
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawLine(m_StartPoint.position, m_StartPoint.position + m_StartPoint.forward * m_MaxDist);
    }
}
