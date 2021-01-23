using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugExtend : MonoBehaviour
{
    public RectTransform m_toExtend;
    public Scrollbar m_scrollBar;
    private Vector2 extendPerHalfSec = new Vector2(500f, 0f);
    // Start is called before the first frame update
    public void StartRecording()
    {
        Debug.Log("Rect Delta size " + m_toExtend.sizeDelta );
        StartCoroutine( ExtendSpace() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ExtendSpace()
    {
        WaitForSeconds wfs = new WaitForSeconds( 0.5f );
        for( int i = 0 ; i < 15 ; i++ )
        {
            m_toExtend.sizeDelta += extendPerHalfSec;
            yield return new WaitForEndOfFrame();
            m_scrollBar.value = 1;
            yield return wfs;
        }
    }
}
