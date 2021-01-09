using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceRecorder : MonoInstance<DanceRecorder>
{
    [Header("References")]
    public TimelineClip m_timelineClipPrefab;
    public TimelineController m_timeIndicator;
    public Transform[] m_track;
    public ChibiAnimator m_chibiAnimator;
    public AnimationLibrary m_animationLibrary;

    [Header("UI")]
    public RecorderButton[] m_recordButton;
    private bool b_isRecording;
    private bool b_isBeingDragged;
    private DanceData m_danceData;

    //System
    private List<TimelineClip> m_pointerTrackClips;
    private List<TimelineClip> m_animationTrackClips;

    private TimelineClip m_latestPointerClip;
    private float m_pointerDuration;
    
    public bool IsRecording{ get{ return b_isRecording; } }
    public bool IsBeingDragged{ get{ return b_isBeingDragged; } set {b_isBeingDragged = value;}  }


    void Start()
    {
        b_isRecording = false;
        b_isBeingDragged = false;
        m_pointerTrackClips = new List<TimelineClip>();
        m_animationTrackClips = new List<TimelineClip>();
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
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
        }
        else if ( Input.GetKeyUp(KeyCode.Space) )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
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
        if( !b_isRecording )
        {
            return;
        }
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 1 ] ).GetComponent<TimelineClip>();
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        temp.ClipType = TimelineClipType.Animation;
        temp.SetAnimationData( p_data, m_animationLibrary.GetAnimationIndex(p_data) );
        temp.SetCallback( m_chibiAnimator.AnimateCharacter );
        // temp.GetComponent<RectTransform>().anchoredPosition = m_timeIndicator.GetCurrentSetPosition();
        m_animationTrackClips.Add(temp);

    }

    public void RecordAnimation( SavedPointerData p_data )
    {
        if( !b_isRecording )
        {
            return;
        }
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 0 ] ).GetComponent<TimelineClip>();
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        temp.ClipType = TimelineClipType.Pointer;
        m_latestPointerClip = temp;
        m_pointerTrackClips.Add(temp);
    }

    public void RemoveClip ( TimelineClip p_timelineClip )
    {
        if( p_timelineClip.ClipType == TimelineClipType.Animation )
        {
            m_animationTrackClips.Remove( p_timelineClip );
        }
        // else if( p_timelineClip.ClipType == TimelineClipType.Pointer )
        else
        {
            m_pointerTrackClips.Remove( p_timelineClip );
        }
    }

    public void ClearTrack()
    {
        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            Destroy( m_pointerTrackClips[i].gameObject );
        }
        for( int i = 0 ; i < m_animationTrackClips.Count ; i++ )
        {
            Destroy( m_animationTrackClips[i].gameObject );
        }
        m_pointerTrackClips.Clear();
        m_animationTrackClips.Clear();
    }

    public void RefreshTrack( float p_zoomLevel)
    {
        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            m_pointerTrackClips[i].RefreshPosition(p_zoomLevel);
            m_pointerTrackClips[i].RefreshWidth(p_zoomLevel);
        }
        for( int i = 0 ; i < m_animationTrackClips.Count ; i++ )
        {
            m_animationTrackClips[i].RefreshPosition(p_zoomLevel);
            m_animationTrackClips[i].RefreshWidth(p_zoomLevel);
        }
    }

    public DanceData ExportDanceData()
    {
        ReadyDanceData();
        return m_danceData;
    }

    public void ReadyDanceData()
    {
        if( m_danceData == null )
        {
            m_danceData = new DanceData();
        }

        m_danceData.TotalDuration = m_timeIndicator.GetTotalDuration();

        // Animation Data
        for( int i = 0 ; i < m_animationTrackClips.Count ; i++ )
        {
            SavedAnimationData temp = new SavedAnimationData();
            temp = m_animationTrackClips[i].SavedAnimationData;
            m_danceData.AnimationDatas.Add(temp);
        }
    }
}

//50 Width for 0.1 second
