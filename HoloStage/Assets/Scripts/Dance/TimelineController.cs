using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    //Settings
    private static float TIME_MARGIN_FOR_EXTEND = 2f;
    private static float TIME_TO_EXTEND = 2f;
    private static float DISTANCE_PER_SECOND = 500f;
    private readonly Vector2 VECTOR_DISTANCE_PER_SECOND = new Vector2(500f, 0f);

    public MusicPlayer m_musicPlayer;
    public ToggleIconButton[] m_playButtons;
    public Slider m_indicator;
    public RectTransform m_timelineRect;
    public Scrollbar m_timelineScrollBar;
    public Text m_label_currentTime;
    public Text m_label_totalTime;
    public Text m_label_zoomLevel;
    
    private bool m_bIsTimeFlowing = false;
    private float m_currentTime;
    private float m_totalTime;
    private int m_zoomLevelIndex;
    private readonly float[] m_zoomLevelArray = {1f,2f,4f};
    public float ZoomLevel {get{ return m_zoomLevelArray[m_zoomLevelIndex]; }}
    public bool IsTimeFlowing {get{ return m_bIsTimeFlowing; }}


    // private bool DanceRecorder.Instance.IsBeingDragged;

    public float GetCurrentTime()
    {
        return m_bIsTimeFlowing? m_currentTime: 0f;;
    }

    public float GetTotalDuration()
    {
        return m_totalTime;
    }

    public Vector2 GetCurrentSetPosition()
    {
        return GetSetPositionOfTime( m_currentTime );
    }
    public Vector2 GetSetPositionOfTime( float p_time )
    {
        return p_time*VECTOR_DISTANCE_PER_SECOND*ZoomLevel;
    }

    public static float ConvertTimeToPosition( float p_duration, float p_zoomLevel = 1f )
    {
        return p_duration * DISTANCE_PER_SECOND * p_zoomLevel ;
    }

    public static float ConvertDurationToWidth( float p_duration, float p_zoomLevel = 1f )
    {
        return p_duration * DISTANCE_PER_SECOND * p_zoomLevel ;
    }

    public void SetCurrentTime( Slider p_slider )
    {
        m_currentTime = p_slider.value * m_totalTime;
    }

    public void OnIndicatorPressed()
    {
        DanceRecorder.Instance.IsBeingDragged = true;
        if( m_bIsTimeFlowing )
        {
            PauseTimeFlow();
        }
    }
    public void OnIndicatorReleased()
    {
        DanceRecorder.Instance.IsBeingDragged = false;
        float sliderToTime = m_indicator.value * m_totalTime;
        m_currentTime = sliderToTime;
        DanceRecorder.Instance.RepositionIndex( m_currentTime );
        m_musicPlayer.OverrideSliderValue( sliderToTime, true );
    }

    public void OverrideIndicatorValue( float p_time )
    {
        m_currentTime = p_time;
        m_indicator.value = p_time/m_totalTime;
    }

    public void OnIndicatorValueChanged()
    {
        m_label_currentTime.text = Utils.FloatTimeToFormattedString( m_indicator.value * m_totalTime, true );
        if( DanceRecorder.Instance.IsBeingDragged )
        {
            float sliderToTime = m_indicator.value * m_totalTime;
            // m_currentTime = sliderToTime;
            m_musicPlayer.OverrideSliderValue( sliderToTime );
        }
    }

    public void OnTotalTimeValueChanged()
    {
        m_label_totalTime.text = Utils.FloatTimeToFormattedString( m_totalTime, true );
    }

    public void UpdateIndicatorPosition()
    {
        m_indicator.value = m_currentTime/m_totalTime;
    }

    public void UpdateTimelineDisplay()
    {
        m_timelineRect.anchoredPosition = ( (m_currentTime-(2/ZoomLevel )  )* -VECTOR_DISTANCE_PER_SECOND ) * ZoomLevel;
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
        for( int i = 0 ; i < m_playButtons.Length; i++ )
        {
            m_playButtons[i].SetIcon(m_bIsTimeFlowing);
        }
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
        for( int i = 0 ; i < m_playButtons.Length; i++ )
        {
            m_playButtons[i].SetIcon(m_bIsTimeFlowing);
        }
        m_musicPlayer.ForcePause();
        DanceRecorder.Instance.SortClipOrder();
    }

    public void StopTimeFlow()
    {
        Debug.LogError("Stop Time Flow");
        m_bIsTimeFlowing = false;
        m_currentTime = 0;
        UpdateIndicatorPosition();
        DanceRecorder.Instance.SortClipOrder();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_bIsTimeFlowing = false;
        // DanceRecorder.Instance.IsBeingDragged = false;
        m_currentTime = 0;
        m_zoomLevelIndex = 0;
        m_totalTime = m_timelineRect.sizeDelta.x/(DISTANCE_PER_SECOND*ZoomLevel);
        OnTotalTimeValueChanged();
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

    // private void FixedUpdate() 
    // {
    //     if( DanceRecorder.Instance.IsBeingDragged )
    //     {
    //         float sliderToTime = m_indicator.value * m_totalTime;
    //         m_currentTime = sliderToTime;
    //         m_musicPlayer.OverrideSliderValue( sliderToTime );
    //     }
    // }

    private void ExtendTimeline()
    {
        if(  DanceRecorder.Instance.IsRecording && m_totalTime - m_currentTime < TIME_MARGIN_FOR_EXTEND )
        {
            // m_timelineRect.sizeDelta += Time.deltaTime * VECTOR_DISTANCE_PER_SECOND;
            // m_totalTime += Time.deltaTime;
            m_timelineRect.sizeDelta += TIME_TO_EXTEND * VECTOR_DISTANCE_PER_SECOND * ZoomLevel;
            m_totalTime += TIME_TO_EXTEND;
            OnTotalTimeValueChanged();
        }
    }

    public void SetTimelineLength ( float p_totalTime )
    {
        m_timelineRect.sizeDelta = p_totalTime * VECTOR_DISTANCE_PER_SECOND * ZoomLevel;
        m_totalTime = p_totalTime;
        OnTotalTimeValueChanged();
    }

    private void FinishTimeline()
    {
        if( !DanceRecorder.Instance.IsRecording && m_currentTime >= m_totalTime )
        {
            PauseTimeFlow();
        }
    }

    public void OnZoomPress(int p_value)
    {
        if( p_value == 0 )
        {
            m_zoomLevelIndex = 0;
        }
        //Zoom in
        else if( p_value > 0 )
        {
            if( m_zoomLevelIndex < m_zoomLevelArray.Length-1 )
            {
                m_zoomLevelIndex++;
            }
        }
        //Zoom out
        else if( p_value < 0 )
        {
            if( m_zoomLevelIndex > 0 )
            {
                m_zoomLevelIndex--;
            }
        }
        m_label_zoomLevel.text = $"x{ZoomLevel}.00";
        m_timelineRect.sizeDelta = m_totalTime * VECTOR_DISTANCE_PER_SECOND * ZoomLevel;
        DanceRecorder.Instance?.RefreshTrack(ZoomLevel );
    }

}
