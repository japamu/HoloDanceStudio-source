using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARToggle : MonoBehaviour
{
    public Text m_confirmationScreenMessage;
    public Text m_confirmationButton;
    private bool isOnARScene;
    private static string AR_SCENE_NAME = "Stage_AR-X";
    private static string NONAR_SCENE_NAME = "Stage_0";
    private static string START_AR_MODE = "Start AR Mode?";
    private static string EXIT_AR_MODE = "Exit AR Mode?";
    private static string MSG_START = "Start";
    private static string MSG_EXIT = "Exit";
    // Start is called before the first frame update
    void Start()
    {
        isOnARScene = (SceneManager.GetActiveScene().name == AR_SCENE_NAME);
        if( isOnARScene )
        {
            //Start AR Mode?
            m_confirmationScreenMessage.text = EXIT_AR_MODE;
            m_confirmationButton.text = MSG_EXIT;
        }
        else
        {
            m_confirmationScreenMessage.text = START_AR_MODE;
            m_confirmationButton.text = MSG_START;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress()
    {
        if( !isOnARScene )
        {
            //Go to AR Scene
            SceneManager.LoadScene(AR_SCENE_NAME,LoadSceneMode.Single);
        }
        else
        {
            //Go to Non AR Scene
            SceneManager.LoadScene(NONAR_SCENE_NAME,LoadSceneMode.Single);
        }
    }

    public void OnResetPress()
    {
        if( !isOnARScene )
        {
            //Go to AR Scene
            SceneManager.LoadScene(AR_SCENE_NAME,LoadSceneMode.Single);
        }
    }
}
