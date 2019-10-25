using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRRayButton : MonoBehaviour
{

  [SerializeField] private Button m_attacnedButton;

    private Color m_NormalColor;
    private Color m_HighLightColor;

    private int m_in_ControllerCount;
    private bool m_intaractable;
    // Start is called before the first frame update
    void Start()
    {
        m_intaractable = true;
        m_NormalColor = m_attacnedButton.colors.normalColor;
        m_HighLightColor = m_attacnedButton.colors.highlightedColor;
    }

    public void ToggleHoverButton(bool i_enable)
    {
        if (m_intaractable)
        {
            m_in_ControllerCount += i_enable ? 1 : -1;
            ColorBlock colors = m_attacnedButton.colors;
            colors.normalColor = m_in_ControllerCount > 0 ? m_HighLightColor : m_NormalColor;
            m_attacnedButton.colors = colors;
        }
    }

    public void VRClickButton()
    {
        if(m_intaractable)
        m_attacnedButton.onClick.Invoke();
    }

    public void ToggleEnableButton(bool i_enable)
    {
        m_intaractable = i_enable;
        m_attacnedButton.interactable = m_intaractable;
    }
}
