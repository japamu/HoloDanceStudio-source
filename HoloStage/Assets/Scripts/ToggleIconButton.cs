using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIconButton : MonoBehaviour
{
    public Image m_buttonIcon;
    public Sprite[] m_iconSet;

    public void SetIcon ( bool p_value )
    {
        int iconIndex = p_value? 1 : 0;
        m_buttonIcon.sprite = m_iconSet[iconIndex];
    }
}
