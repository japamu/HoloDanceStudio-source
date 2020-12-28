using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationButton : MonoBehaviour
{
    public Image m_animImage;
    private AnimationData m_animData;
    private Action<AnimationData> m_callback;
    public AnimationData GetAnimationData()
    {
        return m_animData;
    }
    public void SetAnimationData( AnimationData p_animData )
    {
        m_animData = p_animData;
        if( p_animData != null )
        {
            m_animImage.sprite = m_animData.m_image;
            m_animImage.enabled = true;
        }
        else
        {
            m_animImage.enabled = false;
        }
    }
    public void SetCallback( Action<AnimationData> p_callback )
    {
        m_callback = p_callback;
    }
    public void ButtonPress()
    {
        if( m_callback!= null )
        {
            m_callback.Invoke( m_animData );
        }
    }
}
