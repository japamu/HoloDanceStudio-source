using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFramerateAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if( Utils.IsMobile() )
        {
            Application.targetFrameRate = 60;
        }
    }

}
