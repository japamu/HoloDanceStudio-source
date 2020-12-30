using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControlManager : MonoBehaviour
{
    [Header("Movement")]
    public bl_Joystick m_joystick;
    public ConstrainedFollow m_spineIK_controller;
    // Start is called before the first frame update
    void Start()
    {
        if ( Utils.IsMobile() )
        {
            m_joystick.gameObject.SetActive( true );
            if( m_spineIK_controller != null && m_joystick != null )
            {
                m_spineIK_controller.SetJoystick( m_joystick );
            }
        }
        else
        {
            m_joystick.gameObject.SetActive( false );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
