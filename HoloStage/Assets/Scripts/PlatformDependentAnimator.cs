using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDependentAnimator : MonoBehaviour
{
    public Animator m_animator;
    public string animstate_pc;
    public string animstate_android;

    void OnEnable()
    {
        if( Utils.IsMobile() )
        {
            m_animator.Play( animstate_android );
        }
        else
        {
            m_animator.Play( animstate_pc );
        }
    }

}
