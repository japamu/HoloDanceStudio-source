using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeySystem : MonoBehaviour
{
    [Header("Settings")]
    public HotkeyButton m_buttonPrefab;
    public HotkeyLibrary m_hotkeyLibrary;
    public ChibiAnimator m_chibiAnimator;
    [Header("UI")]

    public Transform[] m_parentHotkeyBar;
    public GameObject m_hotkeyWindow;
    public SelectAnimationWindow m_selAnimationWindow;

    [Header("Mobile UI")]
    public HotkeyButton m_buttonPrefabMobile;
    public ControlButton m_controlButtonPrefab;
    public Transform[] m_parentHotkeyBarMobile;
    public Transform m_parentMobileControlPanel;
    public int m_maxMobileHotkey = 6;


    private Dictionary<KeyCode,HotkeyButton> m_hotkeyButtons;
    public Dictionary<KeyCode,HotkeyButton> HotkeyButtons{ get{return m_hotkeyButtons;} }
    private List<ControlButton> m_controlButtons;



    // Start is called before the first frame update
    void Start()
    {
        m_hotkeyButtons = new Dictionary<KeyCode,HotkeyButton>();
        m_controlButtons = new List<ControlButton>();
        if( Utils.IsMobile() )
        {
            SetupMobilecontrolPanel();
        }
        SetupHotkeyWindow();
        m_chibiAnimator.SetHotkeySystem( this );
    }

    // Update is called once per frame
    void Update()
    {
        if( m_hotkeyWindow.activeInHierarchy )
        {
            foreach( KeyValuePair<KeyCode,HotkeyButton> item in HotkeyButtons )
            {
                if( Input.GetKeyDown( item.Key ) )
                {
                    OpenSelectAnimationWindow( item.Value , true );
                }
            }
        }        
    }

    public void SetupMobilecontrolPanel()
    {
        ControlButton temp;
        for( int i = 0; i < m_maxMobileHotkey; i++ )
        {
            temp = Instantiate( m_controlButtonPrefab, m_parentMobileControlPanel ) as ControlButton;
            temp.SetCallback( m_chibiAnimator.AnimateCharacter );
            temp.SetHotkey( m_hotkeyLibrary.Numbers[i] );
            m_controlButtons.Add( temp );
        }
        
    }
    public void SetupHotkeyWindow()
    {
        HotkeyButton temp;
        if( Utils.IsMobile() )
        {
            for( int i = 0; i < m_maxMobileHotkey; i++ )
            {
                temp = Instantiate( m_buttonPrefabMobile, m_parentHotkeyBarMobile[0] ) as HotkeyButton;
                temp.SetHotkey( m_hotkeyLibrary.Numbers[i] );
                temp.SetCallback( OpenSelectAnimationWindow );
                temp.SetControlButton( m_controlButtons[i] );
                m_hotkeyButtons.Add( m_hotkeyLibrary.Numbers[i].KeyCode ,temp );
            }
        }
        else
        {
            for( int i = 0; i < m_hotkeyLibrary.Alpha.Count; i++ )
            {
                temp = Instantiate( m_buttonPrefab, m_parentHotkeyBar[0] ) as HotkeyButton;
                temp.SetHotkey( m_hotkeyLibrary.Alpha[i] );
                temp.SetCallback( OpenSelectAnimationWindow );
                m_hotkeyButtons.Add( m_hotkeyLibrary.Alpha[i].KeyCode ,temp );
            }
            for( int i = 0; i < m_hotkeyLibrary.Numbers.Count; i++ )
            {
                temp = Instantiate( m_buttonPrefab, m_parentHotkeyBar[1] ) as HotkeyButton;
                temp.SetHotkey( m_hotkeyLibrary.Numbers[i] );
                temp.SetCallback( OpenSelectAnimationWindow );
                m_hotkeyButtons.Add( m_hotkeyLibrary.Numbers[i].KeyCode ,temp );
            }
        }
        
    }

    public void OpenSelectAnimationWindow( HotkeyButton m_button)
    {
        m_selAnimationWindow.gameObject.SetActive(true);
        m_selAnimationWindow.SetSelectedHotKey( m_button );
    }
    public void OpenSelectAnimationWindow( HotkeyButton m_button, bool p_cannotOpen)
    {
        if( p_cannotOpen == true && m_selAnimationWindow.gameObject.activeInHierarchy == true )
        {
            OpenSelectAnimationWindow(m_button);
        }
    }

    public void ToggleHotkeyWindow( bool p_state )
    {
        m_hotkeyWindow.gameObject.SetActive( p_state );
        if( p_state == false )
        {
            m_selAnimationWindow.gameObject.SetActive( p_state );
        }
    }
}
