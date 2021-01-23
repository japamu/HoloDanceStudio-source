using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicPlayer : MonoBehaviour
{
    public TimelineController m_timeIndicator;
    public AudioSource m_audioSource;
    public Text m_label_filename;
    public ToggleIconButton[] m_playButtons;
    // public Button m_handleButton;
    public Slider m_musicScrubber;
    public ToggleIconButton m_muteButton;
    public Text m_label_timeCurrent;
    public Text m_label_timeTotal;
    private TimeSpan m_timespan_current;
    private TimeSpan m_timespan_total;
    private string m_fileName;
    private float m_musicSpeed;
    // private bool DanceRecorder.Instance.IsBeingDragged;

    // Start is called before the first frame update
    void Start()
    {
        m_musicScrubber.interactable = false;
        for( int i = 0 ; i < m_playButtons.Length; i++ )
        {
            m_playButtons[i].SetIcon(false);
        }
        m_musicSpeed = 1;
        // DanceRecorder.Instance.IsBeingDragged = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( m_audioSource.isPlaying || DanceRecorder.Instance.IsBeingDragged )
        {
            UpdateMusicUI();
        }
    }

    public void SetAudioFile( AudioClip p_clip, string p_filename = "" )
    {
        m_audioSource.clip = p_clip;
        m_fileName = p_filename;
        m_label_filename.text = m_fileName;
    }

    public void CompletedLoading()
    {
        
        m_timespan_current = TimeSpan.FromSeconds( 0 );
        m_timespan_total = TimeSpan.FromSeconds( m_audioSource.clip.length );
        m_musicScrubber.interactable = true;
        m_label_timeCurrent.text =  Utils.TimeSpanToFormattedString( m_timespan_current );
        m_label_timeTotal.text =  Utils.TimeSpanToFormattedString( m_timespan_total );
        // m_label_timeTotal.text = m_timespan_total.ToString(@"mm\:ss");
        m_audioSource.Pause();
        m_audioSource.time = 0;
        DanceRecorder.Instance.m_timeIndicator.SetTimelineLength( m_audioSource.clip.length );

    }

    public void UpdateMusicUI()
    {
        if( DanceRecorder.Instance.IsBeingDragged )
        {
            if( m_audioSource.clip != null )
            {
                float sliderToTime = m_musicScrubber.value * m_audioSource.clip.length;
                m_timespan_current = TimeSpan.FromSeconds( sliderToTime );
                m_label_timeCurrent.text =  Utils.TimeSpanToFormattedString( m_timespan_current );
            }
        }
        else
        {
            m_timespan_current = TimeSpan.FromSeconds( m_audioSource.time );
            m_label_timeCurrent.text =  Utils.TimeSpanToFormattedString( m_timespan_current );
            m_musicScrubber.value = m_audioSource.time / m_audioSource.clip.length;
        }
        // m_timespan_current = TimeSpan.FromSeconds( m_audioSource.time );
        // m_label_timeCurrent.text =  Utils.TimeSpanToFormattedString( m_timespan_current );
        // m_musicScrubber.value = m_audioSource.time / m_audioSource.clip.length;
    }

    public void OnPressPlayButton()
    {
        //Toggle Between pause and play
        if( m_audioSource.clip != null )
        {
            if( m_audioSource.isPlaying )
            {
                m_audioSource.Pause();
            }
            else
            {
                m_audioSource.Play();
            }
            for( int i = 0 ; i < m_playButtons.Length; i++ )
            {
                m_playButtons[i].SetIcon(m_audioSource.isPlaying);
            }
        }
    }
    public void OnPressStopButton()
    {
        if( m_audioSource.clip != null )
        {
            m_audioSource.Pause();
            m_audioSource.time = 0;
            for( int i = 0 ; i < m_playButtons.Length; i++ )
            {
                m_playButtons[i].SetIcon(m_audioSource.isPlaying);
            }
            UpdateMusicUI();
        }
        else{
            for( int i = 0 ; i < m_playButtons.Length; i++ )
            {
                m_playButtons[i].SetIcon(false);
            }
        }
        DanceRecorder.Instance.RepositionIndex( 0 );
    }

    public void OnSliderChanged()
    {
        UpdateMusicUI();
        if( DanceRecorder.Instance.IsBeingDragged )
        {
            float sliderToTime = m_musicScrubber.value * m_audioSource.clip.length;
            // m_currentTime = sliderToTime;
            m_timeIndicator.OverrideIndicatorValue( sliderToTime );
        }
    }

    public void OnHandlePressed()
    {
        DanceRecorder.Instance.IsBeingDragged = true;
    }

    public void OnHandleReleased()
    {
        DanceRecorder.Instance.IsBeingDragged = false;
        float sliderToTime = m_musicScrubber.value * m_audioSource.clip.length;
        m_audioSource.time = sliderToTime;
        DanceRecorder.Instance.RepositionIndex( sliderToTime );
        m_timeIndicator.OverrideIndicatorValue( sliderToTime );
    }

    public void OverrideSliderValue( float p_seconds, bool p_bApplyTotime = false )
    {
        if( p_bApplyTotime )
        {
            m_audioSource.time = p_seconds;
        }
        if( m_audioSource.clip != null )
        {
            m_musicScrubber.value = p_seconds/m_audioSource.clip.length;
            UpdateMusicUI();
        }
        Debug.LogError($"override! + {p_seconds}");
    }

    public void OnVolumeSliderChanged(Slider p_slider)
    {
        // float sliderToTime = p_slider.value * m_audioSource.clip.length;
        m_audioSource.volume = p_slider.value;
    }

    public void OnPressMuteButton()
    {
        m_audioSource.mute = !m_audioSource.mute;
        m_muteButton.SetIcon( m_audioSource.mute );
    }

    public void ForcePause()
    {
        //Toggle Between pause and play
        if( m_audioSource.clip != null )
        {
            if( m_audioSource.isPlaying )
            {
                m_audioSource.Pause();
            }
        }
    }
}
