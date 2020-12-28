using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{
    public Animator m_animator;
    public HotkeySystem m_hotkeySystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( !Input.anyKeyDown )
        {
            return;
        }
        foreach( KeyValuePair<KeyCode,HotkeyButton> item in m_hotkeySystem.HotkeyButtons )
        {
            if( Input.GetKeyDown( item.Key ) && item.Value.GetAnimationData()!= null )
            {
                AnimationData current = item.Value.GetAnimationData();
                m_animator.Play( current.m_name, current.m_animationLayer );
            }
        }

    }
}
