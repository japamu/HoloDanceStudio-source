using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
    public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }
    public bool HasInput { get { return input != Vector2.zero; } }

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    [SerializeField, Range(0.01f, 1)]private float SmoothTime = 0.5f;//return to default position speed

    public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }
    private float smoothTime { get { return (1 - (SmoothTime)); } }


    [SerializeField] private float handleRange = 1;
    [SerializeField, Range(0.5f, 4)] private float OnPressScale = 1.5f;//return to default position speed
    [SerializeField, Range(0.1f, 5)]private float Duration = 1;

    [SerializeField] private float deadZone = 0;
    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;
    private Vector3 currentVelocity;
    private Vector3 PressScaleVector;
    private int lastId = -2;



    protected virtual void Start()
    {
        HandleRange = handleRange;
        DeadZone = deadZone;
        baseRect = GetComponent<RectTransform>();;
        if (transform.root.GetComponent<Canvas>() != null)
        {
            canvas = transform.root.GetComponent<Canvas>();
        }
        else if (transform.root.GetComponentInChildren<Canvas>() != null)
        {
            canvas = transform.root.GetComponentInChildren<Canvas>();
        }
        else
        {
            Debug.LogError("Required at lest one canvas for joystick work.!");
            this.enabled = false;
            return;
        }

        Vector2 center = new Vector2(0.5f, 0.5f);
        background.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;

        PressScaleVector = new Vector3(OnPressScale, OnPressScale, OnPressScale);
    }
    void Update()
    {
        // DeathArea = CenterReference.position;
        //If this not free (not touched) then not need continue
        if ( HasInput )
            return;

        //Return to default position with a smooth movement
        handle.anchoredPosition = Vector3.SmoothDamp(handle.anchoredPosition, Vector3.zero, ref currentVelocity, smoothTime);
        //When is in default position, we not need continue update this
        if (Vector3.Distance(handle.anchoredPosition, Vector3.zero) < .1f)
        {
            handle.anchoredPosition = Vector3.zero;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (lastId == -2)
        {
            lastId = eventData.pointerId;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(true));
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == lastId)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            //Get Position of current touch
            Vector3 position = JoystickUtils.TouchPosition(canvas,GetTouchID);

            Vector2 bgposition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (position - (Vector3)bgposition) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }
    }

    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == AxisOptions.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == AxisOptions.Vertical)
            input = new Vector2(0f, input.y);
    }

    private float SnapFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == AxisOptions.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == AxisOptions.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            else if (snapAxis == AxisOptions.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == lastId)
        {
            lastId = -2;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(false));
            input = Vector2.zero;
            // handle.anchoredPosition = Vector2.zero;
        }
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    IEnumerator ScaleJoysctick(bool increase)
    {
        float _time = 0;

            while (_time < Duration)
            {
                Vector3 v = handle.localScale;
            if (increase)
            {
                v = Vector3.Lerp(handle.localScale, PressScaleVector, (_time / Duration));
            }
            else
            {
                v = Vector3.Lerp(handle.localScale, Vector3.one, (_time / Duration));
            }
            handle.localScale = v;
                _time += Time.deltaTime;
                yield return null;
            }
    }

    public int GetTouchID
    {
        get
        {
            //find in all touches
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastId)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}

public enum AxisOptions { Both, Horizontal, Vertical }