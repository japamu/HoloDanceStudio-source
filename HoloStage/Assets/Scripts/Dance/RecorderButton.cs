using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecorderButton : MonoBehaviour
{
    public GameObject m_recordEffect;
    public void SetIconToRecording( bool p_isRecording )
    {
        m_recordEffect.SetActive( p_isRecording );
    }
}
