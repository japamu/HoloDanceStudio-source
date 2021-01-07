using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineClip : MonoBehaviour
{
    private static float DEFAULT_WIDTH = 50;
    [SerializeField] private RectTransform m_rectTransform;
    private float m_timestamp;

    private SavedPointerData m_savedPointerData;
    private SavedAnimationData m_savedAnimationData;
    

    private void Start() 
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void SetTimestamp( float p_timestamp, Vector2 p_position )
    {
        m_timestamp = p_timestamp;
        SetPosition(p_position);
    }

    public void SetPosition( Vector2 p_position )
    {
        m_rectTransform.anchoredPosition = p_position;
    }

    public void SetWidth( float p_width )
    {
        if( p_width > DEFAULT_WIDTH)
        {
            Vector2 size = m_rectTransform.sizeDelta;
            size.x = p_width;
            m_rectTransform.sizeDelta = size;
        }
    }


}
