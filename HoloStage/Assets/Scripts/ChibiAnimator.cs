using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiAnimator : MonoBehaviour
{
    public Animator m_animator;
    private HotkeySystem m_hotkeySystem;
    public void SetHotkeySystem( HotkeySystem p_hotkeySystem )
    {
        m_hotkeySystem = p_hotkeySystem;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_hotkeySystem == null )
        {
            return;
        }
        if( !Input.anyKeyDown )
        {
            return;
        }
        foreach( KeyValuePair<KeyCode,HotkeyButton> item in m_hotkeySystem.HotkeyButtons )
        {
            if( Input.GetKeyDown( item.Key ) && item.Value.GetAnimationData()!= null )
            {
                AnimateCharacter( item.Value.GetAnimationData() );
            }
        }

    }

    public void AnimateCharacter( AnimationData p_animData )
    {
        m_animator.Play( p_animData.m_name, p_animData.m_animationLayer );
        DanceRecorder.Instance.RecordAnimation( p_animData );
    }
}
