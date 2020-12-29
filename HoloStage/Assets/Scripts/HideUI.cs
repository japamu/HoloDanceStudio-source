using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public CanvasGroup[] m_hideCanvasGroup;

    public void ToggleHideUI( bool p_toggle )
    {
        for( int i = 0 ; i < m_hideCanvasGroup.Length ; i++ )
        {
            m_hideCanvasGroup[i].alpha = p_toggle?1:0;
            if( m_hideCanvasGroup[i].GetComponent<ToggleInteractability>() != null )
            {
                m_hideCanvasGroup[i].GetComponent<ToggleInteractability>().Toggle( p_toggle );
            }
        }
    }
}
