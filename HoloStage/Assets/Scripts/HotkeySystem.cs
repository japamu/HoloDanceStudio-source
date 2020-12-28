using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeySystem : MonoBehaviour
{
    [Header("Settings")]
    public HotkeyButton m_buttonPrefab;
    public HotkeyLibrary m_hotkeyLibrary;
    [Header("UI")]

    public Transform[] m_parentHotkeyBar;
    public GameObject m_hotkeyWindow;
    public SelectAnimationWindow m_selAnimationWindow;

    private Dictionary<KeyCode,HotkeyButton> m_hotkeyButtons;
    public Dictionary<KeyCode,HotkeyButton> HotkeyButtons{ get{return m_hotkeyButtons;} }



    // Start is called before the first frame update
    void Start()
    {
        m_hotkeyButtons = new Dictionary<KeyCode,HotkeyButton>();
        SetupHotkeyWindow();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void SetupHotkeyWindow()
    {
        HotkeyButton temp;
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

    public void OpenSelectAnimationWindow( HotkeyButton m_button)
    {
        m_selAnimationWindow.gameObject.SetActive(true);
        m_selAnimationWindow.SetSelectedHotKey( m_button );
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
