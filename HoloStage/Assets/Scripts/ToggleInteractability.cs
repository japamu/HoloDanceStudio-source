using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractability : MonoBehaviour
{
    private CanvasGroup m_canvasGrp;
    
    // Start is called before the first frame update
    void Start()
    {
        m_canvasGrp = GetComponent<CanvasGroup>();   
    }
    public void Toggle( bool p_interactable )
    {
        if( m_canvasGrp != null )
        {
            m_canvasGrp.interactable = p_interactable;
        }
    }
}
