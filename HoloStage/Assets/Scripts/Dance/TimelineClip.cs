using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TimelineClipType{
    Pointer = 0,
    Animation
}

public class TimelineClip : MonoBehaviour
{
    private static float DEFAULT_WIDTH = 50;
    public static float DEFAULT_DURATION = 0.05f;
    public static float HOLD_DURATION_DEL = 0.5f;
    [SerializeField] private RectTransform m_rectTransform;
    private float m_timestamp;
    public float TimeStamp{ get{return m_timestamp;} }
    private float m_duration;
    private float m_deleteCounter;
    private bool m_bIsPressingDown;
    public float Duration{ get{return m_duration;} }
    public float TimeStampFinish{ get{return m_timestamp+m_duration;} }
    public Image m_icon;

    private float m_localTimer;
    private int m_localIndex;
    private AnimationData m_animData;
    private List<Vector3> m_pointerPositions;
    private SavedPointerData m_savedPointerData;
    private SavedAnimationData m_savedAnimationData;
    public AnimationData AnimationData{ get{return m_animData;} }
    public SavedPointerData SavedPointerData{ get{return m_savedPointerData;} }
    public SavedAnimationData SavedAnimationData{ get{return m_savedAnimationData;} }
    private bool m_pointerIsOver; 
    private Action<AnimationData> m_callback;

    private TimelineClipType m_timelineClip;
    public TimelineClipType ClipType{ get{return m_timelineClip;} set{m_timelineClip=value;} }
    public bool GivenLastPoint{ get{  return m_localIndex >= (m_pointerPositions.Count); } }

    public void PointerOver( bool p_pointerIsOver )
    {
        m_pointerIsOver = p_pointerIsOver;
    }
    

    private void Start() 
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_bIsPressingDown = false;
    }

    public void SetCallback( Action<AnimationData> p_callback )
    {
        m_callback = p_callback;
    }
    public void SetAnimationData( AnimationData p_animData, int p_animIndex )
    {
        m_icon.sprite = DanceRecorder.Instance.m_iconLibrary.m_animationIcon[p_animData.m_animationLayer];
        m_animData = p_animData;
        m_savedAnimationData = new SavedAnimationData();
        m_savedAnimationData.animType = p_animData.m_animationLayer;
        m_savedAnimationData.animIndex = p_animIndex;
        m_savedAnimationData.timestamp = m_timestamp;
    }

    public void SetPointerData( Vector3 p_pointerData )
    {
        m_icon.sprite = DanceRecorder.Instance.m_iconLibrary.m_pointerIcon;
        m_pointerPositions = new List<Vector3>();
        m_pointerPositions.Add( p_pointerData );
        m_savedPointerData = new SavedPointerData();
        m_savedPointerData.timestamp = m_timestamp;
    }
    public void SetPointerListData ( List<Vector2> p_pointerDatas )
    {
        m_icon.sprite = DanceRecorder.Instance.m_iconLibrary.m_pointerIcon;
        m_pointerPositions = new List<Vector3>();
        for( int i = 0 ; i < p_pointerDatas.Count ; i++ )
        {
            Debug.LogError($"pointerData:{i}");
            Debug.LogError($"pointerDatav2:{p_pointerDatas[i]}");
            m_pointerPositions.Add( p_pointerDatas[i] );
        }
        m_savedPointerData = new SavedPointerData();
        m_savedPointerData.timestamp = m_timestamp;
        FinishPointerData();
    }

    public void FinishPointerData()
    {
        for( int i = 0 ; i < m_pointerPositions.Count ; i++ )
        {
            m_savedPointerData.pointerPositions.Add( m_pointerPositions[i] );
        }
    }

    public void SetTimestamp( float p_timestamp, Vector2 p_position )
    {
        m_timestamp = p_timestamp;
        SetPosition(p_position);
        m_localTimer = 0;
    }

    public void SetPosition( Vector2 p_position )
    {
        m_rectTransform.anchoredPosition = p_position;
    }

    public void RefreshPosition( float p_zoomLevel = 1 )
    {
        Vector2 pos = m_rectTransform.anchoredPosition;
        pos.x = TimelineController.ConvertTimeToPosition( m_timestamp, p_zoomLevel );
        m_rectTransform.anchoredPosition = pos;
    }

    public void RefreshWidth( float p_zoomLevel = 1 )
    {
        SetWidth( TimelineController.ConvertDurationToWidth( m_duration, p_zoomLevel ) );
    }

    public void SetDuration( float p_dur, float p_zoomLevel )
    {
        if( p_dur > DEFAULT_DURATION )
        {
            m_duration = p_dur;
        }
        SetWidth( TimelineController.ConvertDurationToWidth( p_dur, p_zoomLevel ) );
    }

    public void SetWidth( float p_width )
    {
        if( p_width > DEFAULT_WIDTH)
        {
            Vector2 size = m_rectTransform.sizeDelta;
            size.x = p_width;
            m_rectTransform.sizeDelta = size;
        }
    }

    public void AddPoint( Vector3 p_pointerData, float p_time )
    {
        //p_time = 0.12f
        float offsetVal = p_time - m_localTimer;
        //offsetval = 0.12f
        if( offsetVal > DEFAULT_DURATION )
        {
            m_pointerPositions.Add( p_pointerData );
            m_localTimer = p_time - (offsetVal-DEFAULT_DURATION);
        }
    }

    public void ResetLocalIndex( float p_time)
    {
        m_localIndex = 0;
        if( m_timelineClip == TimelineClipType.Pointer )
        {
            //Clip is after current time
            if( p_time > m_timestamp )
            {
                //clip is outside time
                if( p_time > m_timestamp+m_duration )
                {
                    m_localIndex = m_pointerPositions.Count;
                }
                else
                {
                    //find the correct index
                    while( p_time >= m_timestamp + ( (float)m_localIndex*DEFAULT_DURATION )  )
                    {
                        m_localIndex++;
                    }
                }
            }
            //Clip is before current time
            else if ( p_time < m_timestamp )
            {
                m_localIndex = 0;
            }
        }
    }

    public bool GivePoint( float p_time )
    {
        //Check if time is in middle
        //m_localIndex = (int)Mathf.Floor((p_time - m_timestamp)/0.1f);

        if(  p_time >= m_timestamp + ( (float)m_localIndex*DEFAULT_DURATION ) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetPoint()
    {
        // float offsetVal = p_time - m_localTimer;
        // float offsetVal = p_time;
        Vector3 val = m_pointerPositions[m_localIndex];
        m_localIndex++;
        return val;
        //offsetval = 0.12f
        // if( offsetVal > DEFAULT_DURATION )
        // {
        //     m_localTimer = p_time - (offsetVal-DEFAULT_DURATION);
        //     m_localIndex++;

        //     if( m_localIndex >= m_pointerPositions.Count )
        //     {
        //         p_isLast = true;
        //     }
        //     return val;
        // }
        // return Vector3.zero;
    }

    private void Update()
    {
        // if( Input.GetKeyDown(KeyCode.Delete) && m_pointerIsOver )
        if(  !Utils.IsMobile() && Input.GetMouseButtonDown(1) && m_pointerIsOver )
        {
            RemoveFromTrack();
        }
        if(  Utils.IsMobile() && m_bIsPressingDown && m_pointerIsOver )
        {
            m_deleteCounter += Time.deltaTime;
            if( m_deleteCounter > HOLD_DURATION_DEL )
            {
                RemoveFromTrack();
            }
        }
    }

    public void RemoveFromTrack()
    {
        //Remove this from list
        DanceRecorder.Instance.RemoveClip(this);
        Destroy( this.gameObject );
    }
    float debugTime = 0;
    public void onPress()
    {
        if( m_callback!= null )
        {
            m_callback.Invoke( m_animData );
            Debug.Log( $"Animation Layer:{m_savedAnimationData.animType} | Animation Index:{m_savedAnimationData.animIndex} " );
            //m_savedAnimationData.animType = p_animData.m_animationLayer;
            //m_savedAnimationData.animIndex = p_animIndex;
        }
        else
        {
            // GivePoint( debugTime );
            // Vector3 d = GetPoint();
            // Debug.Log( $"Point: {d}" );
            // debugTime += 0.05f;

            // for( int i = 0 ; i < m_pointerPositions.Count ; i++ )
            // {
            //     Debug.Log($"Point: {m_pointerPositions[i]}");
            // }
        }
        if( Utils.IsMobile() )
        {
            m_deleteCounter = 0;
        }
    }

    public void OnPressMobile( bool p_isDown ) {
        if( !Utils.IsMobile() )
        {
            return;
        }
        m_bIsPressingDown = p_isDown;
        m_deleteCounter = 0;

    }


}
