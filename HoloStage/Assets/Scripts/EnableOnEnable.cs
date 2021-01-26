using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnEnable : MonoBehaviour
{
    public GameObject m_enable;
    // Start is called before the first frame update
    void OnEnable() 
    {
        m_enable.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
