using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if ( Utils.IsMobile() )
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonPress()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
