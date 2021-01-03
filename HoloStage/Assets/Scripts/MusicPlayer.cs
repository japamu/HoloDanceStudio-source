using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource m_audioSource;
    public Text m_label_filename;
    public PlayButton m_playButton;
    public Slider m_musicScrubber;
    public Text m_label_timeCurrent;
    public Text m_label_timeTotal;
    private TimeSpan m_timespan_current;
    private TimeSpan m_timespan_total;
    private string m_fileName;
    private float m_musicSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_musicScrubber.interactable = false;
        m_playButton.SetIcon(false);
        m_musicSpeed = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( m_audioSource.isPlaying )
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

    }

    public void UpdateMusicUI()
    {
        m_timespan_current = TimeSpan.FromSeconds( m_audioSource.time );
        m_label_timeCurrent.text =  Utils.TimeSpanToFormattedString( m_timespan_current );
        m_musicScrubber.value = m_audioSource.time / m_audioSource.clip.length;
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
            m_playButton.SetIcon( m_audioSource.isPlaying );
        }
    }
    public void OnPressStopButton()
    {
        m_audioSource.Stop();
        m_playButton.SetIcon( m_audioSource.isPlaying );
        UpdateMusicUI();
    }

    public void OnSliderChanged(Slider p_slider)
    {
        float sliderToTime = p_slider.value * m_audioSource.clip.length;
        m_audioSource.time = sliderToTime;
    }
}
