using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformDependentImage : MonoBehaviour
{
    public Image m_image;
    public Sprite sprite_pc;
    public Sprite sprite_android;
    // Start is called before the first frame update
    void Start()
    {
        if( Utils.IsMobile() )
        {
            m_image.sprite = sprite_android;
        }
        else
        {
            m_image.sprite = sprite_pc;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
