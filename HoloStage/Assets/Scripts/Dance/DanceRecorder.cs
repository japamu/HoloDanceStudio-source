using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceRecorder : MonoInstance<DanceRecorder>
{
    [Header("References")]
    public TimelineClip m_timelineClipPrefab;
    public TimeIndicator m_timeIndicator;
    public Transform[] m_track;

    [Header("UI")]
    public RecorderButton[] m_recordButton;
    private bool b_isRecording;
    private DanceData m_danceData;

    //System
    private List<TimelineClip> m_pointerTrackClips;
    private List<TimelineClip> m_animationTrackClips;

    private TimelineClip m_latestPointerClip;
    private float m_pointerDuration;

    public bool IsRecording{ get{ return b_isRecording; } }

    void Start()
    {
        b_isRecording = false;
    }
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if( !IsRecording )
        {
            return;
        }
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            RecordAnimation( new SavedPointerData() );
            m_pointerDuration = 0;
        }
        else if ( Input.GetKey(KeyCode.Space) )
        {
            m_latestPointerClip.SetWidth( m_timeIndicator.ConvertDurationToWidth( m_pointerDuration ) );
        }
        else if ( Input.GetKeyUp(KeyCode.Space) )
        {
            m_latestPointerClip.SetWidth( m_timeIndicator.ConvertDurationToWidth( m_pointerDuration ) );
            m_pointerDuration = 0;
        }
        m_pointerDuration += Time.deltaTime;

        
    }

    public void OnRecordButtonPressed()
    {
        b_isRecording = !b_isRecording;
        for( int i = 0 ; i < m_recordButton.Length ; i++ )
        {
            m_recordButton[i].SetIconToRecording( b_isRecording );
        }
    }

    public void RecordAnimation( AnimationData p_data )
    {
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ Random.Range( 0, 2) ] ).GetComponent<TimelineClip>();
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        // temp.GetComponent<RectTransform>().anchoredPosition = m_timeIndicator.GetCurrentSetPosition();

    }

    public void RecordAnimation( SavedPointerData p_data )
    {
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 0 ] ).GetComponent<TimelineClip>();
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        m_latestPointerClip = temp;
    }
}

//50 Width for 0.1 second
