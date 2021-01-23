using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CharacterSetterAR : MonoBehaviour
{
    const string k_FadeOffAnim = "FadeOff";

    //This is in meters irl
    [Header("AR Setup References")]
    public ARPlaneManager m_planeManager;
    public GameObject m_detectUI;
    [Header("Initialize Stage")]
    public ARRaycastManager m_raycastManager;
    public GameObject m_ARObjects;
    public GameObject m_character;
    public CharacterARManipulator m_charARManipulator;
    [Header("Initialize Detecting")]
    public GameObject m_revealUI;
    public GameObject RaycastPointer;
    // public TextMesh m_textm;
    public GameObject EmptyPointerLoc;
    [Header("Setup Values")]
    public float m_minimumAreaSize = 0.5f;
    // public float m_minimumBattleDistance = 1.5f;
    // public float m_maximumBattleDistance = 2.0f;
    // private const float AUTODISTANCE = 2.0f;

    //Internal
    private bool bIsReadyForInvasion = false;
    private bool bHasInitiatedInvasion = false;
    private Color transparent = new Color(0,0,0,0);
    private List<ARRaycastHit> AR_hitResults;
    private Vector2 screenCenter;


    void OnEnable() {
        // m_plane = GetComponent <ARPlane> ();
        m_planeManager.planesChanged += PlanesChanged;
        // m_plane.boundaryChanged += PlaneOnBoundaryChanged;
    }

    // Start is called before the first frame update
    void Start() {
        bIsReadyForInvasion = false;
        bHasInitiatedInvasion = false;
        RaycastPointer.SetActive(false);
        m_revealUI.SetActive(false);
        AR_hitResults = new List<ARRaycastHit>();
        screenCenter = new Vector2( Screen.width/2 , Screen.height/2 );
        // if( m_battleManager == null )
        // {
        //     m_battleManager = GameObject.FindObjectOfType<BattleManager>();
        // }
    }

    // Update is called once per frame
    void Update() {
        //CastRaycast
        AR_hitResults.Clear();
        if( bIsReadyForInvasion && !bHasInitiatedInvasion )
        {
            RaycastPointer.SetActive(true);
            bool hasHit = m_raycastManager.Raycast( screenCenter, AR_hitResults, TrackableType.Planes );
            float dist = AR_hitResults[0].distance;
            // bool isFarEnough = ( dist > m_minimumBattleDistance && dist < m_maximumBattleDistance );
            //Add Distance checker here
            if( hasHit )
            {
                // m_textm.text = dist.ToString("F2");
                RaycastPointer.transform.position = AR_hitResults[0].pose.position;
                RaycastPointer.transform.rotation = AR_hitResults[0].pose.rotation;
                if( Input.GetMouseButtonDown(0) )
                {
                    bHasInitiatedInvasion = true;
                    m_ARObjects.SetActive(true);
                    m_character.transform.position = AR_hitResults[0].pose.position ;
                    m_charARManipulator.InitializeValues();
                    m_revealUI.SetActive(false);
                    RaycastPointer.SetActive(false);
                    HidePlanes();

                }
                // RaycastPointer.transform.LookAt( Vector3.up );
                // m_battleManager.StartGameSession( plane );
            }
            else
            {
                RaycastPointer.SetActive(false);
                //if did not hit will display pointer in front of screen
                // RaycastPointer.transform.position = EmptyPointerLoc.transform.position;
                // RaycastPointer.transform.rotation = EmptyPointerLoc.transform.rotation;
            }
        }

    }
    void PlanesChanged(ARPlanesChangedEventArgs p_obj) 
    {
        ARPlane plane = p_obj.updated[0];
        if( !bIsReadyForInvasion ) 
        {
            float area = CalculatePlaneArea(plane);
            // m_detectUI.UpdateLabel( (area / m_minimumAreaSize) *100 );
            if ( area > m_minimumAreaSize ) 
            {
                Debug.Log("Area gotten enough for game session");
                m_detectUI.SetActive(false);
                m_revealUI.SetActive(true);
                bIsReadyForInvasion = true;
                RaycastPointer.SetActive(true);
                // plane.GetComponent<Renderer>().material.SetColor("_TexTintColor", transparent);
                //Instantiate a fader plane here and start game instance
            }
        }
        m_obj = p_obj;
        if( bHasInitiatedInvasion )
        {
            HidePlanes();
        }
        
    }
    private ARPlanesChangedEventArgs m_obj;

    void HidePlanes()
    {
        for(int i = 0; i < m_obj.added.Count; i++ )
        {
            m_obj.added[i].GetComponent<Renderer>().material.SetColor("_TexTintColor", transparent);
            // obj.added[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < m_obj.updated.Count; i++ )
        {
            m_obj.updated[i].GetComponent<Renderer>().material.SetColor("_TexTintColor", transparent);
            // obj.updated[i].gameObject.SetActive(false);
        }
    }

    private float CalculatePlaneArea(ARPlane p_plane) {
        return p_plane.size.x * p_plane.size.y;
    }
}
