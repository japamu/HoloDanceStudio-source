using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent : MonoBehaviour
{
    public UnityEvent m_event;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeEvent()
    {
        m_event.Invoke();
    }
}
