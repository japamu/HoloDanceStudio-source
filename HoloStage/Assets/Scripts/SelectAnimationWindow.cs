using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAnimationWindow : MonoBehaviour
{
    [Header("Settings")]
    public AnimationButton m_buttonPrefab;
    public AnimationLibrary m_animationLibrary;

    [Header("UI")]
    private HotkeyButton m_selectedHotkey;
    public Text m_selectedKey_label;
    public Image m_selectedKey_icon;
    public Transform m_scrollablePanel_eyes;
    public Transform m_scrollablePanel_mouth;

    private List<AnimationButton> m_animationButtons;
    public List<AnimationButton> AnimationButtons{ get{return m_animationButtons;} }


    // Start is called before the first frame update
    void Start()
    {
        m_animationButtons = new List<AnimationButton>();
        SetupAnimationWindow();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void SetupAnimationWindow()
    {
        AnimationButton temp;
        for( int i = 0; i < m_animationLibrary.eyeAnimation.Length; i++ )
        {
            temp = Instantiate( m_buttonPrefab, m_scrollablePanel_eyes ) as AnimationButton;
            temp.SetAnimationData( m_animationLibrary.eyeAnimation[i] );
            temp.SetCallback( SelectAnimation );
            m_animationButtons.Add( temp );
        }
        for( int i = 0; i < m_animationLibrary.mouthAnimation.Length; i++ )
        {
            temp = Instantiate( m_buttonPrefab, m_scrollablePanel_mouth ) as AnimationButton;
            temp.SetAnimationData( m_animationLibrary.mouthAnimation[i] );
            temp.SetCallback( SelectAnimation );
            m_animationButtons.Add( temp );
        }
        // for( int i = 0; i < m_hotkeyLibrary.Numbers.Count; i++ )
        // {
        //     temp = Instantiate( m_buttonPrefab, m_parentHotkeyBar[1] ) as HotkeyButton;
        //     temp.SetHotkey( m_hotkeyLibrary.Numbers[i] );
        //     temp.SetCallback( OpenSelectAnimationWindow );
        //     m_hotkeyButtons.Add( temp );
        // }
        
    }

    public void SetSelectedHotKey( HotkeyButton p_button )
    {
        m_selectedHotkey = p_button;
        m_selectedKey_label.text = p_button.GetHotkeyData().KeyName;
        if( p_button.GetAnimationData() != null )
        {
            m_selectedKey_icon.enabled = true;
            m_selectedKey_icon.sprite = p_button.GetAnimationData().m_image;
        }
        else
        {
            m_selectedKey_icon.enabled = false;
        }
    }

    public void SelectAnimation( AnimationData p_anim )
    {
        m_selectedHotkey.SetAnimData( p_anim );
        if( p_anim != null )
        {
            m_selectedKey_icon.enabled = true;
            m_selectedKey_icon.sprite = p_anim.m_image;
        }
    }
}
