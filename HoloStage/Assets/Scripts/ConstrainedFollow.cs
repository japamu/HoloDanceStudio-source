using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstrainedFollow : MonoBehaviour
{
    public Transform m_target;
    public Collider2D[] m_bounds;

    private Vector2 m_lastPos = Vector2.zero;
    private Vector2 m_cp;
    private Vector3 m_mousePosition;
    public float m_moveSpeed = 0.1f;
    private Vector2 m_position = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if( m_target == null )
        {
            return;
        }
        if( !Utils.MouseScreenCheck() )
        {
            return;
        }
        m_mousePosition = Input.mousePosition;
        m_mousePosition = Camera.main.ScreenToWorldPoint(m_mousePosition);
        m_position = Vector2.Lerp( m_target.position, m_mousePosition, m_moveSpeed);

        if( (Vector2)m_mousePosition == m_lastPos )
        {
            
            return;
        }
        else
        {
            
            m_cp = (Vector3)m_position + (Vector3.one * 100f);
            int overlap = 0;

            for( int i = 0 ; i < m_bounds.Length; i++ )
            {
                if (m_bounds[i].OverlapPoint(m_position) )
                {
                    overlap++;
                }
                else
                {
                    Vector2 t = m_bounds[i].ClosestPoint(m_position);
                    m_cp =  Vector2.Distance( m_lastPos, t) < Vector2.Distance( m_lastPos, m_cp) ? t:m_cp;
                    // m_cp = Vector2.Lerp( m_target.position, m_cp, m_moveSpeed);
                }
            }
            if( overlap == 0 )
            {
                // Debug.LogError("Not inside bounds");
                m_target.position = m_cp;
            }
            else{
                m_target.position = m_position;
            }
            m_lastPos = m_target.position;
        }
    }

    
}
