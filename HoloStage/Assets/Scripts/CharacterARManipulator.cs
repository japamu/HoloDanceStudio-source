using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterARManipulator : MonoBehaviour
{
    [Header("Other Inputs")]
    public Joystick m_joystick;
    [Header("Character")]
    public Transform m_character;
    public float[] m_presetScale;
    public int initialScaleIndex = 2;
    [Range(0,1)]
    public float deltaMinimum = 0.1f;

    [Header("Debug")]
    public Text debugDeltaMinLabel;

    private int scaleIndex;
    private float pinchIndex;
	private Vector2 m_screenSize;

    private Quaternion m_originalRot;
    private Vector2 initPos = new Vector2(0,0);
    private Vector2 deltaPos = new Vector2(0,0);

    private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private Vector2 initDist = new Vector2(0,0);

    private GameObject m_pinchInstruction;

    public void InitializeValues()
    {
        scaleIndex = initialScaleIndex;
        pinchIndex = 0;
        m_originalRot = Quaternion.identity;
        m_originalRot.eulerAngles = m_character.localRotation.eulerAngles ;
        m_screenSize = new Vector2( Screen.width, Screen.height );
        m_character.localScale = Vector3.one * m_presetScale[scaleIndex];
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if( m_joystick.HasInput )
            return;
        // ProcessSingleTouch();
        ProcessMultitouch();
    }
    private void ProcessSingleTouch()
    {
        if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began )
        {
            initPos = Input.GetTouch(0).position;
        }
        if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved )
        {
            deltaPos = Input.GetTouch(0).position;
            float posXDelta = initPos.x - deltaPos.x;
            if( deltaPos.x > initPos.x && posXDelta > deltaMinimum )
            {
                m_character.Rotate( Vector3.up, 10, Space.Self );
                // initPos = Input.GetTouch(0).position; //This resets the point of reference for swiping
            }
            if( deltaPos.x < initPos.x && posXDelta > deltaMinimum )
            {
                m_character.Rotate( Vector3.up, -10, Space.Self );
            //     initPos = Input.GetTouch(0).position; //This resets the point of reference for swiping
            }
        }
        // if( Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began )
        // {
        //     initPos = Input.GetTouch(0).position;
        // }
    }

    private void ProcessMultitouch()
    {
        if( Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(0).phase == TouchPhase.Began) )
        {
            pinchIndex = 0f;
            initDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //initial distance between finger touches
            curDist = new Vector2(0,0);            
        }
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
        {
            curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
			// prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
            // float frameDelta = curDist.magnitude - prevDist.magnitude;
            // if( Mathf.Abs(frameDelta) < deltaMinimum )
            // {
            //     return;
            // }

            float touchDelta = Mathf.Abs(curDist.magnitude - initDist.magnitude);
            touchDelta/= m_screenSize.magnitude;
			// Zoom out
			// if( touchDelta > pinchIndex * deltaMinimum )
			if( initDist.magnitude < curDist.magnitude && touchDelta > deltaMinimum )
			{
                if( scaleIndex < m_presetScale.Length-1 )
                {
                    pinchIndex+=1.0f;
                    scaleIndex++;
                    m_character.localScale = Vector3.one * m_presetScale[scaleIndex];
                    initDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //This resets the point of reference for pinching
                    PinchInstructionCheck();
                }
				// if(m_character.localScale.x < MAXSCALE && m_character.localScale.y < MAXSCALE)
				// {
				// 	Vector3 scale = new Vector3(m_character.localScale.x + scale_factor, m_character.localScale.y + scale_factor, 1);
				// 	scale.x = (scale.x > MAXSCALE) ? MAXSCALE : scale.x;
				// 	scale.y = (scale.y > MAXSCALE) ? MAXSCALE : scale.y;
				// 	scaleFromPosition(scale,midPoint);
				// }
			}
			//Zoom in
			else if( initDist.magnitude > curDist.magnitude && touchDelta > deltaMinimum )
			{
                if( scaleIndex > 0 )
                {
                    pinchIndex-=1.0f;
                    scaleIndex--;
                    m_character.localScale = Vector3.one * m_presetScale[scaleIndex];
                    initDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //This resets the point of reference for pinching
                    PinchInstructionCheck();
                }
				// if(m_character.localScale.x > MIN_SCALE && m_character.localScale.y > MIN_SCALE)
				// {
				// 	Vector3 scale = new Vector3(m_character.localScale.x + scale_factor*-1, m_character.localScale.y + scale_factor*-1, 1);
				// 	scale.x = (scale.x < MIN_SCALE) ? MIN_SCALE : scale.x;
				// 	scale.y = (scale.y < MIN_SCALE) ? MIN_SCALE : scale.y;
				// 	scaleFromPosition(scale,midPoint);
				// }
			}
        }
    }
    //Debug Functions
    public void DisplayDeltaMinimum()
    {
        debugDeltaMinLabel.text = deltaMinimum.ToString();
    }

    public void AddDeltaMinimum ( float p_val )
    {
        if( deltaMinimum > 0.1f || deltaMinimum < 1f )
        {
            deltaMinimum += p_val;
        }
        DisplayDeltaMinimum();
    }
    
    public void SetPinchInstruction ( GameObject p_object )
    {
        m_pinchInstruction = p_object;
    }

    private void PinchInstructionCheck()
    {
        if( m_pinchInstruction != null )
        {
            int pinchShow = PlayerPrefs.GetInt(Utils.KEY_PINCH);
            pinchShow++;
            PlayerPrefs.SetInt(Utils.KEY_PINCH, pinchShow);
            
            m_pinchInstruction.SetActive(false);
            m_pinchInstruction = null;
        }
    }
}
