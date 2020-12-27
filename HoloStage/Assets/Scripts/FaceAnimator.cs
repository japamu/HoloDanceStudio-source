using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{
    public Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Alpha1) )
        {
            Debug.Log("Pressed");
            m_animator.Play("a_eye_normal", 0 );
        }
        else if( Input.GetKeyDown(KeyCode.Alpha2) )
        {
            Debug.Log("Pressed");
            m_animator.Play("a_eye_playful", 0 );
        }
        if( Input.GetKeyDown(KeyCode.Q) )
        {
            m_animator.Play("a_mouth_smile_large", 1 );
        }
        else if( Input.GetKeyDown(KeyCode.W) )
        {
            m_animator.Play("a_mouth_v", 1 );
        }
        else if( Input.GetKeyDown(KeyCode.E) )
        {
            m_animator.Play("a_mouth_a", 1 );
        }

    }
}
