﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum TimelineClipType{
    Pointer = 0,
    Animation
}

public class TimelineClip : MonoBehaviour
{
    private static float DEFAULT_WIDTH = 50;
    private static float DEFAULT_DURATION = 0.1f;
    [SerializeField] private RectTransform m_rectTransform;
    private float m_timestamp;
    private float m_duration;
    private AnimationData m_animData;
    private SavedPointerData m_savedPointerData;
    private SavedAnimationData m_savedAnimationData;
    public SavedPointerData SavedPointerData{ get{return m_savedPointerData;} }
    public SavedAnimationData SavedAnimationData{ get{return m_savedAnimationData;} }
    private bool m_pointerIsOver; 
    private Action<AnimationData> m_callback;

    private TimelineClipType m_timelineClip;
    public TimelineClipType ClipType{ get{return m_timelineClip;} set{m_timelineClip=value;} }

    public void PointerOver( bool p_pointerIsOver )
    {
        m_pointerIsOver = p_pointerIsOver;
    }
    

    private void Start() 
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void SetCallback( Action<AnimationData> p_callback )
    {
        m_callback = p_callback;
    }
    public void SetAnimationData( AnimationData p_animData, int p_animIndex )
    {
        m_animData = p_animData;
        m_savedAnimationData = new SavedAnimationData();
        m_savedAnimationData.animType = p_animData.m_animationLayer;
        m_savedAnimationData.animIndex = p_animIndex;
        m_savedAnimationData.timestamp = m_timestamp;
    }

    public void SetTimestamp( float p_timestamp, Vector2 p_position )
    {
        m_timestamp = p_timestamp;
        SetPosition(p_position);
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

    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.Delete) && m_pointerIsOver )
        {
            RemoveFromTrack();
        }
    }

    public void RemoveFromTrack()
    {
        //Remove this from list
        DanceRecorder.Instance.RemoveClip(this);
        Destroy( this.gameObject );
    }

    public void onPress()
    {
        if( m_callback!= null )
        {
            m_callback.Invoke( m_animData );
            Debug.Log( $"Animation Layer:{m_savedAnimationData.animType} | Animation Index:{m_savedAnimationData.animIndex} " );
            //m_savedAnimationData.animType = p_animData.m_animationLayer;
            //m_savedAnimationData.animIndex = p_animIndex;
        }
    }


}
