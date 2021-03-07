using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
#endif
public class ARAvailability : MonoBehaviour
{
    public Button[] m_button;
    public GameObject m_ARConfirmationScreen;
    public GameObject m_ARCheckScreen;
    public GameObject m_loadIndicator;
    private bool m_bIsInstalling = false;

#if UNITY_ANDROID

    private void OnEnable()
    {
        // StartCoroutine(ARSession.CheckAvailability());
        // StartCoroutine(AllowARScene());
        // m_button.interactable = false;

        //if AR check has been done
        if( PlayerPrefs.HasKey( Utils.KEY_ARCHECK) )
        {
            //If AR is available
            if( PlayerPrefs.GetInt(Utils.KEY_ARCHECK,0) == 2  )
            {
                GetComponent<ARSession>().enabled = true;
                Debug.Log("AR available");
            }
            else if( PlayerPrefs.GetInt(Utils.KEY_ARCHECK,0) > 0 )
            {
                // GetComponent<ARSession>().enabled = true;
                Debug.Log("AR available but needs update");
            }
            else
            {
                //AR not available for device
                Debug.Log("AR not available");
                SetButtonState(false);
            }


        }
    }

    private void SetButtonState( bool p_state, bool p_show=false )
    {
        Debug.Log("set button state: " + p_state );
        for( int i = 0 ; i < m_button.Length ; i++ )
        {
            m_button[i].interactable = p_state;
            m_button[i].gameObject.SetActive( p_state);
        }
    }

    public void OnButtonPress()
    {
        //If AR Check has not been done
        if( !PlayerPrefs.HasKey( Utils.KEY_ARCHECK) || PlayerPrefs.GetInt( Utils.KEY_ARCHECK) < 2 )
        {
            ShowCheckScreen();
        }
        else
        {
            //If AR check has been done
            ShowConfirmationScreen();
        }
    }

    private void ShowCheckScreen()
    {
        Debug.Log("Show AR Check Screen");
        m_ARCheckScreen.SetActive(true);
    }

    public void CheckScreenOK()
    {
        GetComponent<ARSession>().enabled = true;
        StartCoroutine(ARSession.CheckAvailability());
        StartCoroutine(AllowARScene());
    }


    public void ShowConfirmationScreen()
    {
        Debug.Log("Show AR Confirmation Screen");
        switch( ARSession.state )
        {
            case ARSessionState.Unsupported:
                Debug.Log("AR Unsupported");
                SetButtonState(false);
            break;
            case ARSessionState.NeedsInstall:
                Debug.Log("AR Needs Installing");
                //add coroutine for rotating button
                // StartCoroutine(ARSession.CheckAvailability());
                // ARSession.Install();
                // StartCoroutine(CheckARInstall());
                // m_bIsInstalling = true;
            break;
            case ARSessionState.Installing:
                Debug.Log("AR Installing");
            break;
            case ARSessionState.Ready:
            case ARSessionState.SessionInitializing:
            case ARSessionState.SessionTracking:
                m_bIsInstalling = false;
                Debug.Log("AR Ready");
                m_ARConfirmationScreen.SetActive(true);
            break;
        }
        
    }
 
    IEnumerator AllowARScene()
    {
        while (true)
        {
            while (ARSession.state == ARSessionState.CheckingAvailability ||
                ARSession.state == ARSessionState.None)
            {
                Debug.Log("Waiting...");
                // SetButtonState(false, true);
                yield return null;
            }
            if (ARSession.state == ARSessionState.Unsupported)
            {
                Debug.Log("AR unsupported");
                NotificationScreen.Instance.ShowWindow("AR Unsupported");
                SetButtonState(false);  //Hide AR buttons to unavailable
                PlayerPrefs.SetInt(Utils.KEY_ARCHECK,0);
                yield break;
            }
            while (ARSession.state == ARSessionState.NeedsInstall ||
                ARSession.state == ARSessionState.Installing )
            {
                SetButtonState(true);
                // m_loadIndicator.SetActive(true);
                Debug.Log("AR Installing...");
                PlayerPrefs.SetInt(Utils.KEY_ARCHECK,1);
                yield return null;
            }
            if (ARSession.state > ARSessionState.CheckingAvailability)
            {
                Debug.Log("AR supported");
                NotificationScreen.Instance.ShowWindow("AR Supported, Click AR Button again");
                SetButtonState(true);
                PlayerPrefs.SetInt(Utils.KEY_ARCHECK,2);
                yield break;
            }
        }      
    }
    IEnumerator CheckARInstall()
    {
        while (true)
        {
            while (ARSession.state == ARSessionState.NeedsInstall ||
                ARSession.state == ARSessionState.Installing )
            {
                m_loadIndicator.SetActive(true);
                Debug.Log("AR Installing...");
                PlayerPrefs.SetInt(Utils.KEY_ARCHECK,1);
                yield return null;
            }
            if (ARSession.state >= ARSessionState.Ready)
            {
                Debug.Log("AR Install Complete");
                m_loadIndicator.SetActive(false);
                m_bIsInstalling = false;
                PlayerPrefs.SetInt(Utils.KEY_ARCHECK,2);
                yield break;
            }
        }      
    }
#endif
 
}
