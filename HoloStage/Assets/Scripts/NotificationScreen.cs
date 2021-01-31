using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationScreen : MonoInstance<NotificationScreen>
{
    public TextMeshProUGUI m_notifMessage;
    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
    public void ShowWindow( string p_message )
    {
        m_notifMessage.text = p_message;
        gameObject.SetActive(true);
    }

}
