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

#if UNITY_ANDROID

    private void OnEnable()
    {
        StartCoroutine(ARSession.CheckAvailability());
        StartCoroutine(AllowARScene());
        // m_button.interactable = false;
    }

    private void SetButtonState( bool p_state, bool p_show=false )
    {
        for( int i = 0 ; i < m_button.Length ; i++ )
        {
            m_button[i].interactable = p_state;
            m_button[i].gameObject.SetActive( p_show? true: p_state);
        }
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
                ARSession.Install();
            break;
            case ARSessionState.Installing:
                Debug.Log("AR Installing");
            break;
            case ARSessionState.Ready:
            case ARSessionState.SessionInitializing:
            case ARSessionState.SessionTracking:
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
                SetButtonState(false, true);
                yield return null;
            }
            if (ARSession.state == ARSessionState.Unsupported)
            {
                Debug.Log("AR unsupported");
                SetButtonState(false);
                yield break;
            }
            if (ARSession.state > ARSessionState.CheckingAvailability)
            {
                Debug.Log("AR supported");
                SetButtonState(true);
                yield break;
            }
        }      
    }
#endif
 
}
