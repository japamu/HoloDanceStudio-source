using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HideUI : MonoBehaviour
{
    public ToggleIconButton m_visibilityButton;
    public CanvasGroup m_canvasGroup;
    public CanvasGroup[] m_hideCanvasGroup;
    private bool m_hideState = true;
    public void Start ()
    {
        m_hideState = true;
    }
    public void ToggleHideUI( bool p_toggle )
    {
        m_hideState = p_toggle;
        m_visibilityButton.SetIcon( p_toggle );
        for( int i = 0 ; i < m_hideCanvasGroup.Length ; i++ )
        {
            m_hideCanvasGroup[i].alpha = p_toggle?1:0;
            if( m_hideCanvasGroup[i].GetComponent<ToggleInteractability>() != null )
            {
                m_hideCanvasGroup[i].GetComponent<ToggleInteractability>().Toggle( p_toggle );
            }
        }
        ShowButton();
    }

    public void ShowButton() 
    {
        if( !m_hideState )
        {
            DOTween.Kill( m_canvasGroup );
            m_canvasGroup.alpha = 1;
        }
    }
    public void HideButton() 
    {
        if( !m_hideState )
        {
            m_canvasGroup.DOFade( 0,1.0f);
        }
    }


}
