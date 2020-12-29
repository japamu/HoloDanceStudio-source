using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleBackground : MonoBehaviour
{
    [Header("Settings")]
    public BackgroundLibrary m_bgLibrary;
    public SpriteRenderer m_backgroundRenderer;
    private int m_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_index = 0;
    }

    public void ButtonPress()
    {
        m_index++;
        if( m_index >= m_bgLibrary.backgroundSet.Count )
        {
            m_index = 0;
        }
        SetBackground(m_index);
    }

    public void SetBackground( int p_index )
    {
        m_backgroundRenderer.sprite = m_bgLibrary.backgroundSet[p_index].m_image;
        m_backgroundRenderer.color = m_bgLibrary.backgroundSet[p_index].m_color;
    }
}
