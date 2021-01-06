using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    //Settings
    private static float TIME_MARGIN_FOR_EXTEND = 2f;
    private static float TIME_TO_EXTEND = 2f;
    private static float DISTANCE_PER_SECOND = 500f;
    private readonly Vector2 VECTOR_DISTANCE_PER_SECOND = new Vector2(500f, 0f);

    public ToggleIconButton m_playButton;
    public Slider m_indicator;
    public RectTransform m_timelineRect;
    public Scrollbar m_timelineScrollBar;
    private bool m_bIsTimeFlowing = false;
    private float m_currentTime;
    private float m_totalTime;

    public float GetCurrentTime()
    {
        return m_currentTime;
    }

    public void SetCurrentTime( Slider p_slider )
    {
        m_currentTime = p_slider.value * m_totalTime;
    }

    public void UpdateIndicatorPosition()
    {
        m_indicator.value = m_currentTime/m_totalTime;
    }

    public void UpdateTimelineDisplay()
    {
        m_timelineRect.anchoredPosition = (m_currentTime-2)* -VECTOR_DISTANCE_PER_SECOND;
    }

    public void OnPressPlayButton()
    {
        if( !m_bIsTimeFlowing )
        {
            StartTimeFlow();
        }
        else
        {
            PauseTimeFlow();
        }
        m_playButton.SetIcon( m_bIsTimeFlowing );
    }

    public void StartTimeFlow()
    {
        Debug.LogError("Start Time Flow");
        m_bIsTimeFlowing = true;
    }
    public void PauseTimeFlow()
    {
        Debug.LogError("Pause Time Flow");
        m_bIsTimeFlowing = false;
        m_playButton.SetIcon( m_bIsTimeFlowing );
    }

    public void StopTimeFlow()
    {
        Debug.LogError("Stop Time Flow");
        m_bIsTimeFlowing = false;
        m_currentTime = 0;
        UpdateIndicatorPosition();

    }

    // Start is called before the first frame update
    void Start()
    {
        m_bIsTimeFlowing = false;
        m_currentTime = 0;
        m_totalTime = m_timelineRect.sizeDelta.x/DISTANCE_PER_SECOND;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_bIsTimeFlowing )
        {
            m_currentTime+= Time.deltaTime;
            ExtendTimeline();
            UpdateIndicatorPosition();
            UpdateTimelineDisplay();
            FinishTimeline();
        }
    }

    private void ExtendTimeline()
    {
        if(  DanceRecorder.Instance.IsRecording && m_totalTime - m_currentTime < TIME_MARGIN_FOR_EXTEND )
        {
            // m_timelineRect.sizeDelta += Time.deltaTime * VECTOR_DISTANCE_PER_SECOND;
            // m_totalTime += Time.deltaTime;
            m_timelineRect.sizeDelta += TIME_TO_EXTEND * VECTOR_DISTANCE_PER_SECOND;
            m_totalTime += TIME_TO_EXTEND;
        }
    }

    private void FinishTimeline()
    {
        if( !DanceRecorder.Instance.IsRecording && m_currentTime >= m_totalTime )
        {
            PauseTimeFlow();
        }
    }
}
