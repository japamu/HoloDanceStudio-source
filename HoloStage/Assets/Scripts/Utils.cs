using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Utils
{
    public const string KEY_FIRST_HELP = "hds_openhelp";
    public const int REPEAT_HELP = 3;
    public const string KEY_PINCH = "hds_pinch";
    public const int REPEAT_PINCH = 2;
    public const string KEY_ARCHECK = "hds_ar_check";
    // Start is called before the first frame update
    public static bool MouseScreenCheck(){
        #if UNITY_EDITOR
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1){
            return false;
        }
        #else
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
            return false;
        }
        #endif
        else {
            return true;
        }
    }

    public static bool IsMobile()
    {
        #if UNITY_ANDROID
            return true;
        #else
            return false;
        #endif
    }

    public static string FloatTimeToFormattedString( float p_seconds, bool p_includeDecimal = false )
    {
        TimeSpan ts = TimeSpan.FromSeconds( p_seconds );
        return TimeSpanToFormattedString( ts, p_includeDecimal);
    }

    public static string TimeSpanToFormattedString( TimeSpan p_timespan, bool p_includeDecimal = false )
    {
        if( p_includeDecimal )
        {
            return p_timespan.ToString(@"mm\:ss\.f");

        }
        else
        {
            return p_timespan.ToString(@"mm\:ss");
        }
    }
}
