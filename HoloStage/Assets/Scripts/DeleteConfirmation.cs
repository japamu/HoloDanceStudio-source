using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmation : MonoBehaviour
{
    public Text m_message;
    private Action m_callback;

    public void ShowMessage( string p_msg, Action p_callback = null)
    {
        m_message.text = p_msg;
        SetCallback( p_callback );
        gameObject.SetActive(true);
    }

    public void SetCallback( Action p_callback )
    {
        m_callback = p_callback;
    }
    public void OnConfirm()
    {  
        if( m_callback != null )
        {
            m_callback.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
