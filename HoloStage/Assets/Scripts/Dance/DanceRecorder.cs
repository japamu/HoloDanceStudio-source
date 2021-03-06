using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DanceRecorder : MonoInstance<DanceRecorder>
{
    [Header("References")]
    public TimelineClip m_timelineClipPrefab;
    public TimelineController m_timeIndicator;
    public Transform[] m_track;
    public ChibiAnimator m_chibiAnimator;
    public ConstrainedFollow m_follow;
    public AnimationLibrary m_animationLibrary;
    public IconLibrary m_iconLibrary;

    [Header("UI")]
    public RecorderButton[] m_recordButton;
    public RecordCountdown m_recordCountdown;
    public ToggleGroup m_toggleGroup;
    public DeleteConfirmation m_deleteConfirmation;
    private bool b_recordButtonState;
    private bool b_isRecording;
    private bool b_pointerIsRecording;
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
    public bool IsTimeFlowing{ get{ return m_timeIndicator.IsTimeFlowing; } }
    public bool IsBeingDragged{ get{ return b_isBeingDragged; } set {b_isBeingDragged = value;}  }
    public bool AfterLastRecorded{ get{ return m_trackIndex[1]>= m_pointerTrackClips.Count; }  }


    void Start()
    {
        b_isRecording = false;
        b_recordButtonState = false;
        b_isBeingDragged = false;
        m_pointerTrackClips = new List<TimelineClip>();
        m_animationTrackClips = new List<TimelineClip>();
        b_unsorted = false;
        m_trackIndex[0] = 0;
        m_trackIndex[1] = 0;
        m_recordCountdown.SetCallback( StartRecording );
        m_timeIndicator.e_PlayButtonPress +=OnPlayButtonPress;
    }
    protected override void Awake()
    {
        base.Awake();
    }

    // public void UpdateDanceData()
    // {
    //     DanceData ddata = ExportDanceData();
    //     DanceExporter._DANCEDATA = ddata;
    // }

    private void Update()
    {
        if( IsRecording && IsTimeFlowing)
        {
            PointerRecord();
            MobilePointerRecord();
        }
        if( m_timeIndicator.IsTimeFlowing )
        {
            //Replay Timeline Clip Here
            if( m_trackIndex[0] < m_animationTrackClips.Count )
            {
                if( m_timeIndicator.GetCurrentTime() >= m_animationTrackClips[ m_trackIndex[0] ].TimeStamp )
                {
                    m_chibiAnimator.AnimateCharacter( m_animationTrackClips[ m_trackIndex[0] ].AnimationData , false );
                    m_trackIndex[0]++;
                }
            }
            //Replay Pointer Tracks
            if( !b_pointerIsRecording && m_trackIndex[1] < m_pointerTrackClips.Count )
            {
                if( m_timeIndicator.GetCurrentTime() >= m_pointerTrackClips[ m_trackIndex[1] ].TimeStamp )
                {
                    if ( m_timeIndicator.GetCurrentTime() >= m_pointerTrackClips[ m_trackIndex[1] ].TimeStamp + m_pointerTrackClips[ m_trackIndex[1] ].Duration )
                    {
                        Debug.Log("Skip Track!");
                        m_trackIndex[1]++;
                    }
                    else if( m_pointerTrackClips[ m_trackIndex[1] ].GivePoint( m_timeIndicator.GetCurrentTime() ) )
                    {
                        Vector3 pos = m_pointerTrackClips[ m_trackIndex[1] ].GetPoint();
                        m_follow.FollowPosition( pos );
                        if( m_pointerTrackClips[ m_trackIndex[1] ].GivenLastPoint )
                        {
                            Debug.Log("Next Track!");
                            m_trackIndex[1]++;
                        }
                    }
                }
            }
        }

        
    }

    public void RepositionIndex( float p_time )
    {
        Debug.LogError("TrackIndex reset");

        //if time = 0, then 0
        if( p_time == 0 )
        {
            m_trackIndex[0] = 0;
            m_trackIndex[1] = 0;
        }
        else
        {
            // look for spot 
            for( int i = 0 ; i < m_animationTrackClips.Count ; i++ )
            {
                if( p_time < m_animationTrackClips[i].TimeStamp )
                {
                    m_trackIndex[0] = i;
                    Debug.Log($"Animation Track Reposition Index: {m_trackIndex[0]}");
                    break;
                }
            }
            for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
            {
                if( p_time < m_pointerTrackClips[i].TimeStamp )
                {
                    m_trackIndex[1] = i;

                    Debug.Log($"Pointer Track Reposition Index: {m_trackIndex[1]}");
                    break;
                }
                else if( p_time < m_pointerTrackClips[i].TimeStampFinish )
                {
                    m_trackIndex[1] = i;
                    Debug.Log($"Pointer Track Reposition Index: {m_trackIndex[1]}");
                    break;
                }
            }
        }

        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            m_pointerTrackClips[i].ResetLocalIndex( p_time );
        }
        
    }

    private void PointerRecord()
    {
        if( Utils.IsMobile() )
        {
            return;
        }
        if( Input.GetMouseButtonDown(1) )
        {
            RecordAnimation( m_follow.PointerPosition );
            m_pointerDuration = 0;
            b_pointerIsRecording = true;
        }
        else if ( Input.GetMouseButton(1) )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
            m_pointerDuration += Time.deltaTime;
            b_pointerIsRecording = true;
        }
        else if ( Input.GetMouseButtonUp(1) )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
            m_latestPointerClip.FinishPointerData();
            OnFinishedPointerRecord();
            m_pointerDuration = 0;
            b_pointerIsRecording = false;
        }
        
    }
    private void MobilePointerRecord()
    {
        if( !Utils.IsMobile() )
        {
            return;
        }
        if( m_follow.Joystick.HasInput && !b_pointerIsRecording )
        {
            RecordAnimation( m_follow.PointerPosition  );
            m_pointerDuration = 0;
            b_pointerIsRecording = true;
        }
        else if ( m_follow.Joystick.HasInput )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
            m_pointerDuration += Time.deltaTime;
            b_pointerIsRecording = true;
        }
        else if ( !m_follow.Joystick.HasInput && b_pointerIsRecording )
        {
            // m_latestPointerClip.SetWidth( TimelineController.ConvertDurationToWidth( m_pointerDuration, m_timeIndicator.ZoomLevel ) );
            m_latestPointerClip.SetDuration(  m_pointerDuration, m_timeIndicator.ZoomLevel );
            // m_latestPointerClip.AddPoint( m_follow.PointerPosition, m_pointerDuration );
            m_latestPointerClip.FinishPointerData();
            OnFinishedPointerRecord();
            m_pointerDuration = 0;
            b_pointerIsRecording = false;
        }
        
    }

    public void OnFinishedPointerRecord()
    {
        //put code here for trimming clips that are overlapping by new timeline clip
        List<TimelineClip> clipToTrim = new List<TimelineClip>();

        float startTime = m_latestPointerClip.TimeStamp;
        float endTime = m_timeIndicator.GetCurrentTime();

        //Find Clips to trim
        for( int i = 0 ; i < m_pointerTrackClips.Count; i++ )
        {
            if(  m_pointerTrackClips[i] != m_latestPointerClip 
                && m_pointerTrackClips[i].TimeStamp > startTime 
                && m_pointerTrackClips[i].TimeStamp < endTime )
            {
                Debug.LogError("trim clip before :" + i);
                clipToTrim.Add(m_pointerTrackClips[i]);
            }
        }

        //Trim clips found
        for( int i = 0 ; i < clipToTrim.Count; i++ )
        {
            clipToTrim[i].TrimClipBefore( startTime, endTime, m_timeIndicator.ZoomLevel );
        }
        // if( m_pointerTrackClips.Count > m_trackIndex[1] && m_pointerTrackClips[ m_trackIndex[1]  ] != null )
        // {
        //     if( m_pointerTrackClips[ m_trackIndex[1]  ].TimeStamp < m_timeIndicator.GetCurrentTime() )
        //     {
        //         Debug.Log("Trimming Current Animation");
        //         m_pointerTrackClips[ m_trackIndex[1]  ].TrimClip(m_timeIndicator.GetCurrentTime(), m_timeIndicator.ZoomLevel );
        //     }
        // }
    }

    public void OnPlayButtonPress()
    {
        if( b_recordButtonState )
        {
            m_recordCountdown.CancelCountdown();
            SetRecordingState ( true );
        }
    }

    public void OnRecordButtonPressed()
    {
        b_recordButtonState = !b_recordButtonState;
        if( b_recordButtonState )
        {
            m_recordCountdown.StartCountdown();
        }
        else
        {
            m_recordCountdown.CancelCountdown();
            SetRecordingState ( false );
        }
        // b_isRecording = !b_isRecording;
        // for( int i = 0 ; i < m_recordButton.Length ; i++ )
        // {
        //     m_recordButton[i].SetIconToRecording( b_isRecording );
        // }
    }

    private void StartRecording()
    {
        SetRecordingState ( true );
        m_timeIndicator.OnPressPlayButton();
    }

    private void SetRecordingState( bool p_state )
    {
        b_isRecording = p_state;
        for( int i = 0 ; i < m_recordButton.Length ; i++ )
        {
            m_recordButton[i].SetIconToRecording( p_state );
        }
    }

    public void RecordAnimation( AnimationData p_data )
    {
        if( !b_isRecording || !IsTimeFlowing)
        {
            return;
        }
        float _time = m_timeIndicator.GetCurrentTime();
        Vector2 _pos = m_timeIndicator.GetCurrentSetPosition();
        RecordAnimationFromData( p_data, _time, _pos );
    }

    public void RecordAnimationFromData( AnimationData p_data , float p_time, Vector2 p_pos )
    {

        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 1 ] ).GetComponent<TimelineClip>();
        temp.SetToggleGroup( m_toggleGroup );
        temp.SetTimestamp( p_time, p_pos );
        temp.ClipType = TimelineClipType.Animation;
        temp.SetAnimationData( p_data, m_animationLibrary.GetAnimationIndex(p_data) );
        temp.SetCallback( m_chibiAnimator.AnimateCharacter );
        if( b_unsorted == false && m_animationTrackClips.Count > 0 && temp.TimeStamp < m_animationTrackClips.Last<TimelineClip>().TimeStamp )
        {
            b_unsorted = true;
        }
        m_animationTrackClips.Add(temp);

    }

    public void RecordAnimation( Vector3 p_data )
    {
        if( !b_isRecording || !IsTimeFlowing )
        {
            return;
        }
        //Cut Current Timeline Clip if it exists
        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            if( m_pointerTrackClips[i].TimeStamp < m_timeIndicator.GetCurrentTime() && m_pointerTrackClips[i].TimeStampFinish > m_timeIndicator.GetCurrentTime() )
            {
                Debug.Log("Trimming Current Animation");
                m_pointerTrackClips[ m_trackIndex[1] ].TrimClip(m_timeIndicator.GetCurrentTime(), m_timeIndicator.ZoomLevel );
            }
        }
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 0 ] ).GetComponent<TimelineClip>();
        temp.SetToggleGroup( m_toggleGroup );
        temp.SetTimestamp( m_timeIndicator.GetCurrentTime(), m_timeIndicator.GetCurrentSetPosition() );
        temp.ClipType = TimelineClipType.Pointer;
        temp.SetPointerData( p_data );

        m_latestPointerClip = temp;
        if( b_unsorted == false && m_pointerTrackClips.Count > 0 && temp.TimeStamp < m_pointerTrackClips.Last<TimelineClip>().TimeStamp )
        {
            b_unsorted = true;
        }
        m_pointerTrackClips.Add(temp);
        Debug.Log("Add new Clip");

    }

    public void RecordAnimationFromData( List<Vector2> p_pointerDatas, float p_time, Vector2 p_pos  )
    {
        TimelineClip temp = Instantiate( m_timelineClipPrefab.gameObject, m_track[ 0 ] ).GetComponent<TimelineClip>();
        temp.SetToggleGroup( m_toggleGroup );
        temp.SetTimestamp( p_time , p_pos );
        temp.ClipType = TimelineClipType.Pointer;
        temp.SetPointerListData( p_pointerDatas );
        temp.SetDuration( TimelineClip.DEFAULT_DURATION * p_pointerDatas.Count, m_timeIndicator.ZoomLevel );
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
        // Pointer Data
        for( int i = 0 ; i < m_pointerTrackClips.Count ; i++ )
        {
            SavedPointerData temp = new SavedPointerData();
            temp = m_pointerTrackClips[i].SavedPointerData;
            m_danceData.PointerDatas.Add(temp);
        }
    }

    public void ImportDanceData( DanceData p_danceData )
    {
        m_danceData = p_danceData;
        RepopulateTrackFromLoading();
    }
    public void RepopulateTrackFromLoading()
    {
        m_timeIndicator.SetTimelineLength( m_danceData.TotalDuration );
        Debug.LogError($"Total Duration : {m_danceData.TotalDuration}");
        for( int i = 0 ; i < m_danceData.AnimationDatas.Count ; i++ )
        {
            SavedAnimationData temp = m_danceData.AnimationDatas[i];
            AnimationData tempAnim= m_animationLibrary.GetAnimationData( temp.animType, temp.animIndex );
            RecordAnimationFromData( tempAnim, temp.timestamp, m_timeIndicator.GetSetPositionOfTime(temp.timestamp));
        }
        for( int i = 0 ; i < m_danceData.PointerDatas.Count ; i++ )
        {
            SavedPointerData temp = m_danceData.PointerDatas[i];
            Debug.LogError($"SavedPointerData{temp.pointerPositions.Count}");
            // AnimationData tempAnim= m_animationLibrary.GetAnimationData( temp.animType, temp.animIndex );
            RecordAnimationFromData( temp.pointerPositions , temp.timestamp, m_timeIndicator.GetSetPositionOfTime(temp.timestamp));
        }
    }

    public void DeleteButtonPress()
    {
        if( m_toggleGroup.ActiveToggles().Count() > 0 )
        {
            m_toggleGroup.GetFirstActiveToggle().GetComponent<TimelineClip>().RemoveFromTrack();
        }
        else
        {
            m_deleteConfirmation.ShowMessage("Clear track?", ClearTrack );
        }
    }
}

//50 Width for 0.1 second
