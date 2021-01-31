using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHelpWindows : MonoBehaviour
{
    public GameObject[] HelpScreens;
    private bool bHasTurnedOffThisSession;
    // Start is called before the first frame update
    void Start()
    {
        int helpShow = PlayerPrefs.GetInt(Utils.KEY_FIRST_HELP);
        if( helpShow < Utils.REPEAT_HELP )
        {
            for( int i = 0 ; i < HelpScreens.Length ; i++ )
            {
                HelpScreens[i].SetActive(true);
            }
        }
        bHasTurnedOffThisSession = false;
    }

    public void OnCloseHelpWindow()
    {
        if( bHasTurnedOffThisSession )
        {
            return;
        }
        
        int helpShow = PlayerPrefs.GetInt(Utils.KEY_FIRST_HELP);
        if( helpShow < Utils.REPEAT_HELP )
        {
            helpShow++;
            PlayerPrefs.SetInt(Utils.KEY_FIRST_HELP, helpShow);
            bHasTurnedOffThisSession = true;
        }
    }

}
