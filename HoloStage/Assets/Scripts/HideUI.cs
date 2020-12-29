using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public CanvasGroup[] m_hideCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleHideUI( bool p_toggle )
    {
        for( int i = 0 ; i < m_hideCanvasGroup.Length ; i++ )
        {
            m_hideCanvasGroup[i].alpha = p_toggle?1:0;
        }
    }
}
