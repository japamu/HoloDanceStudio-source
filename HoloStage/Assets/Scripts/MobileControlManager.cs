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
        if( m_spineIK_controller != null && m_joystick != null )
        {
            m_spineIK_controller.SetJoystick( m_joystick );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
