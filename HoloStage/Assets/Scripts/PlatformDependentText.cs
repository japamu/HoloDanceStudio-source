using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformDependentText : MonoBehaviour
{
    public Text m_text;
    public string message_pc;
    public string message_android;
    // Start is called before the first frame update
    void Start()
    {
        if( Utils.IsMobile() )
        {
            m_text.text = message_android;
        }
        else
        {
            m_text.text = message_pc;
        }
    }

}
