using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MobileRestrictions : MonoBehaviour
{
    public GameObject[] m_enabled;
    public GameObject[] m_disabled;
    public Button[] m_restricted;
    // Start is called before the first frame update
    void Start()
    {
        if( Utils.IsMobile() )
        {
            // for( int i  = 0 ; i < m_restricted.Length ; i++ )
            // {
            //     m_restricted[i].interactable = false;
            // }
            for( int i  = 0 ; i < m_disabled.Length ; i++ )
            {
                m_disabled[i].SetActive( false );
            }
            for( int i  = 0 ; i < m_enabled.Length ; i++ )
            {
                m_enabled[i].SetActive( true );
            }
        }
        else
        {
            for( int i  = 0 ; i < m_disabled.Length ; i++ )
            {
                m_disabled[i].SetActive( true );
            }
            for( int i  = 0 ; i < m_enabled.Length ; i++ )
            {
                m_enabled[i].SetActive( false );
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
