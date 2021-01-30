using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAdjust : MonoBehaviour
{
    public Transform m_targetCamera;
    public Transform pos_default;
    public Transform pos_menu;

    public GameObject[] m_windowsToObserve;
    private bool m_bMenuOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CheckWindows()
    {
        // if( Utils.IsMobile() )
        // {
        //     return;
        // }
        int check = 0;
        for( int i = 0 ; i < m_windowsToObserve.Length; i++ )
        {
            if( m_windowsToObserve[i].activeInHierarchy )
            {
                check++;
            }
        }
        if( check > 0 )
        {
            MoveToPosMenu();
        }
        else
        {
            MoveToPosDefault();
        }
    }

    void MoveToPosMenu()
    {
        if( m_bMenuOpen == false )
        {
            DOTween.Kill( m_targetCamera );
            m_targetCamera.DOMove( pos_menu.position, 1.0f );
            m_bMenuOpen = true;
        }
    }

    void MoveToPosDefault()
    {
        if( m_bMenuOpen == true )
        {
            DOTween.Kill( m_targetCamera );
            m_targetCamera.DOMove( pos_default.position, 1.0f );
            m_bMenuOpen = false;
        }

    }
}
