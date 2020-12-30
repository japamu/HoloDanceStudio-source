using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeyButton : MonoBehaviour
{
    public Image m_animImage;
    public Text m_hotkeyLabel;
    private AnimationData m_animData;
    private HotkeyData m_hotkeyData;
    private Action<HotkeyButton> m_callback;
    private ControlButton m_controlButton;

    // Start is called before the first frame update
    public HotkeyData GetHotkeyData()
    {
        return m_hotkeyData;
    }
    public AnimationData GetAnimationData()
    {
        return m_animData;
    }

    public void SetHotkey( HotkeyData p_hotkeyData )
    {
        m_animImage.enabled = false;
        m_hotkeyData = p_hotkeyData;
        m_hotkeyLabel.text = m_hotkeyData.KeyName;

    }

    public void SetControlButton( ControlButton p_controlButton )
    {
        m_controlButton = p_controlButton;
    }

    public void SetAnimationData( AnimationData p_animData )
    {
        m_animData = p_animData;
        if( m_controlButton != null )
        {
            m_controlButton.SetAnimationData(m_animData);
        }
        if( m_animData != null )
        {
            m_animImage.sprite = m_animData.m_image;
            m_animImage.enabled = true;
        }
        else
        {
            m_animImage.enabled = false;
        }
    }

    public void SetCallback (Action<HotkeyButton> p_callback)
    {
        m_callback = p_callback;
    }

    public void ButtonPress() 
    {
        if( m_callback == null)
        {
            Debug.Log("No callback assigned");
            return;
        }
        //This should open the Animation Window
        m_callback.Invoke(this);
    }

}
