using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacebarController : MonoBehaviour
{
    public ActionEvent playButtonEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            playButtonEvent.InvokeEvent();
        }
    }
}
