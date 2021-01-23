using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RecordCountdown : MonoBehaviour
{
    public CanvasGroup m_canvasGroup;
    public Text m_countdownText;
    public Image m_fill;
    private Action m_callback;
    private float _countdownDuration = 3.0f;
    private bool m_bToStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCallback ( Action p_callback )
    {
        m_callback = p_callback;
    }

    public void StartCountdown()
    {
        m_bToStart = true;
        m_canvasGroup.alpha = 1;
        m_fill.fillAmount = 1;
        m_countdownText.text = "3";
        m_fill.DOFillAmount( 0, 1 ).SetLoops(3,LoopType.Restart);
        m_countdownText.DOCounter( 3, 1, _countdownDuration ).OnComplete( CompletedCountdown );
        // m_countdownText.DOText( "0", _countdownDuration, true );
        // m_countdownText
    }

    private void CompletedCountdown()
    {
        if( !m_bToStart )
            return;
        m_canvasGroup.alpha = 0;
        if( m_callback != null )
        {
            m_callback.Invoke();
        }
    }

    public void CancelCountdown()
    {
        m_bToStart = false;
        m_canvasGroup.alpha = 0;
        DOTween.Kill( m_fill );
        DOTween.Kill( m_countdownText );
    }
}
