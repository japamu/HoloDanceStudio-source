using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public Image m_buttonIcon;
    public Sprite[] m_iconSet;

    public void SetIcon ( bool p_isPlaying )
    {
        int iconIndex = p_isPlaying? 1 : 0;
        m_buttonIcon.sprite = m_iconSet[iconIndex];
    }
}
