using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeDelay : MonoBehaviour
{
    public float m_delay;
    public string m_sceneDestination;
    // Start is called before the first frame update
    void Start()
    {
        Invoke( nameof(SceneChange), m_delay );
    }

    void SceneChange()
    {
        SceneManager.LoadScene( m_sceneDestination, LoadSceneMode.Single);
    }
}
