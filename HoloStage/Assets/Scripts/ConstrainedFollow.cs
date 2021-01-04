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

    private bool m_bIsMobile;
    private Joystick m_joystick;
    public bool m_debugMobile;

    // Start is called before the first frame update
    void Start()
    {
        m_bIsMobile = Utils.IsMobile();
        #if UNITY_EDITOR
        if( m_debugMobile )
            m_bIsMobile = true;
        #endif
    }

    public void SetJoystick( Joystick p_joystick )
    {
        m_joystick = p_joystick;
    }

    // Update is called once per frame
    void Update()
    {
        
        if( m_target == null )
        {
            return;
        }
        if( !Utils.MouseScreenCheck() && !m_bIsMobile )
        {
            return;
        }
        if( m_bIsMobile )
        {
            //Control Scheme for Mobile
            if( m_joystick != null )
            {
                // Debug.LogError($"Horizontal: {m_joystick.Horizontal} | Vertical: {m_joystick.Vertical}" );
                Vector2 joystickPos = new Vector2( m_joystick.Horizontal, m_joystick.Vertical );
                m_target.position = GetPositionInBounds(joystickPos);
            }
        }
        else
        {
            if( !Input.GetMouseButton(0) )
            {
                return;
            }
            //Control Scheme for Desktop Exe
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

    Vector2 GetPositionInBounds( Vector2 p_position )
    {
        // float maxVector = 5.0f;
        // p_position /= maxVector;
        // float boundsMultiplier = Vector2.Distance( m_bounds[0].bounds.center, m_bounds[0].bounds.max );
        float boundsX_mult = Mathf.Abs(m_bounds[0].bounds.center.x - m_bounds[0].bounds.max.x);
        float boundsY_mult = Mathf.Abs(m_bounds[0].bounds.center.y - m_bounds[0].bounds.max.y);
        Vector2 newPos = new Vector2( p_position.x * boundsX_mult, p_position.y * boundsY_mult );
        return (Vector2)m_bounds[0].bounds.center + newPos;
        // return (Vector2)m_bounds[0].bounds.center +(p_position * boundsMultiplier);

    }

    
}
