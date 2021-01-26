using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARToggle : MonoBehaviour
{
    private bool isOnARScene;
    private static string AR_SCENE_NAME = "Stage_AR-X";
    private static string NONAR_SCENE_NAME = "Stage_0";
    // Start is called before the first frame update
    void Start()
    {
        isOnARScene = (SceneManager.GetActiveScene().name == AR_SCENE_NAME);
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
}
