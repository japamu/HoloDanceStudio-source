using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonebop : MonoBehaviour
{
    public Transform m_targetBone;
    public ConstrainedFollow m_follow;
    public Vector2 m_limit;
    private float ratio;
    private float targetY;
    private Vector3 bonePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void MoveBone()
    {
        float lower = m_follow.m_bounds[0].bounds.min.y;
        float upper = m_follow.m_bounds[0].bounds.max.y;
        float target = m_follow.m_target.position.y;
        ratio = (target-lower)/(upper-lower);
        float targetY = m_limit.x + ( (m_limit.y-m_limit.x)*ratio);
        bonePos = m_targetBone.localPosition;
        bonePos.x = targetY;
        m_targetBone.localPosition = bonePos;
    }

    // Update is called once per frame
    void Update()
    {
        if( !Utils.MouseScreenCheck() )
        {
            return;
        }
        MoveBone();

    }
}
