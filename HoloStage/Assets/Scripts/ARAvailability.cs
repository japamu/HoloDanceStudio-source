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
    public Button m_button;
#if UNITY_ANDROID

    private void OnEnable()
    {
        StartCoroutine(ARSession.CheckAvailability());
        StartCoroutine(AllowARScene());
        // m_button.interactable = false;
    }
 
    IEnumerator AllowARScene()
    {
        while (true)
        {
            while (ARSession.state == ARSessionState.CheckingAvailability ||
                ARSession.state == ARSessionState.None)
            {
                Debug.Log("Waiting...");
                m_button.interactable = false;
                yield return null;
            }
            if (ARSession.state == ARSessionState.Unsupported)
            {
                Debug.Log("AR unsupported");
                gameObject.SetActive( false );
                m_button.interactable = false;
                yield break;
            }
            if (ARSession.state > ARSessionState.CheckingAvailability)
            {
                Debug.Log("AR supported");
                gameObject.SetActive( true );
                m_button.interactable = true;
                yield break;
            }
        }      
    }
#endif
 
}
