using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public GameObject m_confirmationWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) )
        {
            m_confirmationWindow.SetActive(true);
        }
    }

    public void CloseSoftware()
    {
        Application.Quit();
    }
}
