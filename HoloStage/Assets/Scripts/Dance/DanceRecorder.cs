using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceRecorder : MonoInstance<DanceRecorder>
{
    public RecorderButton[] m_recordButton;
    private bool b_isRecording;
    private DanceData m_danceData;

    public bool IsRecording{ get{ return b_isRecording; } }

    void Start()
    {
        b_isRecording = false;
    }
    protected override void Awake()
    {
        base.Awake();
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
        
    }

    public void RecordAnimation( SavedFollowData p_data )
    {
        
    }
}

//50 Width for 0.1 second
