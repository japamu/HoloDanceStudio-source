using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DanceRecorder : MonoInstance<DanceRecorder>
{
    [Header("References")]
    public TimelineClip m_timelineClipPrefab;
    public TimelineController m_timeIndicator;
    public Transform[] m_track;
    public ChibiAnimator m_chibiAnimator;
    public ConstrainedFollow m_follow;
    public AnimationLibrary m_animationLibrary;

    [Header("UI")]
    public RecorderButton[] m_recordButton;
    private bool b_isRecording;
    private bool b_isBeingDragged;
    private bool b_unsorted;
    private DanceData m_danceData;

    //System
    private List<TimelineClip> m_pointerTrackClips;
    private List<TimelineClip> m_animationTrackClips;

    private TimelineClip m_latestPointerClip;
    private int[] m_trackIndex = {0,0};
    private int m_clipPointIndex = 0;
    private float m_pointerDuration;
    private Vector3 m_lastPoint;
    
    public bool IsRecording{ get{ return b_isRecording; } }
    public bool IsBeingDragged{ get{ return b_isBeingDragged; } set {b_isBeingDragged = value;}  }


    void Start()
    {
        b_isRecording = false;
        b_isBeingDragged = false;
        m_pointerTrackClips = new List<TimelineClip>();
        m_animationTrackClips = new List<TimelineClip>();
        b_unsorted = false;
        m_trackIndex[0] = 0;
        m_trackIndex[1] = 0;
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
        else
        {
            PointerRecord();
        }
        if( m_timeIndicator.IsTimeFlowing )
        //Replay Timeline Clip Here
        if( m_trackIndex[0] < m_animationTrackClips.Count )
        {
            if( m_timeIndicator.GetCurrentTime() >= m_animationTrackClips[ m_trackIndex[0] ].TimeStamp )
            {
                m_chibiAnimator.AnimateCharacter( m_animationTrackClips[ m_trackIndex[0] ].AnimationData , false );
                m_trackIndex[0]++;
            }
        }
        if( m_trackIndex[1] < m_pointerTrackClips.Count )
        {
            if( m_timeIndicator.GetCurrentTime() >= m_pointerTrackClips[ m_trackIndex[1] ].TimeStamp )
            {
                Vector3 pos = m_pointerTrackClips[ m_trackIndex[1] ].GetPoint( m_timeIndicator.GetCurrentTime() );
                m_follow.FollowPosition( pos );
                m_trackIndex[1]++;

                // if( m_lastPoint != pos )
                // {
                //     Debug.Log("New Position");
                //     m_follow.FollowPosition( pos, isLast );
                //     if( isLast )
                //     {
                //         m_trackIndex[1]++;
                //         m_clipPointIndex = 0;
                //     }
                // }
                // m_lastPoint = pos;
                // if( m_pointerTrackClips[ m_trackIndex[1] ]. )
            }
        }

        
    }

    public void RepositionIndex( float p_time )
    {
        Debug.LogError("TrackIndex reset");
        m_trackIndex[0] = 0;
        m_trackIndex[1] = 0;
        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            m_pointerTrackClips[i].ResetLocalIndex();
        }
        
    }

    private void PointerRecord()
    {
        if( Input.GetMouseButtonDown(1) )
        {
            RecordAnimation( m_follow.PointerPosition );
            m_pointerDuration = 0;
        }
        else if ( Input.GetMouseButton(1) )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
        }
        else if ( Input.GetMouseButtonUp(1) )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
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
        if( b_unsorted == false && m_animationTrackClips.Count > 0 && temp.TimeStamp < m_animationTrackClips.Last<TimelineClip>().TimeStamp )
        {
            b_unsorted = true;
        }
        m_animationTrackClips.Add(temp);

    }

    //call this every .1 seconds to record movement
    public void RecordAnimation( Vector3 p_data )
    {
        if( !b_isRecording )
        {
            return;
        }
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 0 ] ).GetComponent<TimelineClip>();
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        temp.ClipType = TimelineClipType.Pointer;
        temp.SetPointerData( p_data );

        m_latestPointerClip = temp;
        if( b_unsorted == false && m_pointerTrackClips.Count > 0 && temp.TimeStamp < m_pointerTrackClips.Last<TimelineClip>().TimeStamp )
        {
            b_unsorted = true;
        }
        m_pointerTrackClips.Add(temp);
    }

    public void RemoveClip ( TimelineClip p_timelineClip )
    {
        b_unsorted = true;
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
        SortClipOrder();
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

    // public void ResetSortedTag()
    // {
    //     b_unsorted = true;
    // }
    public void SortClipOrder()
    {
        if( b_unsorted )
        {
            b_unsorted = false;
            Debug.LogError("Sorting");
            m_animationTrackClips = m_animationTrackClips.OrderBy( o=>o.TimeStamp ).ToList();
        }
        else
        {
            Debug.LogError("Already Sorted");
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
