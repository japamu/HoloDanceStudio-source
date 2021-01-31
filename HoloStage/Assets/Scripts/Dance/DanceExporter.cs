#if UNITY_ANDROID && !UNITY_EDITOR
using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class DanceExporter : MonoBehaviour
{
    [SerializeField] DanceRecorder m_danceRecorder;
    public static DanceData _DANCEDATA;

    // Start is called before the first frame update
    void Start()
    {
        if( _DANCEDATA != null )
        {
            m_danceRecorder.ClearTrack();
            m_danceRecorder.ImportDanceData( _DANCEDATA );
        }
		FileBrowser.AddQuickLink( "Save Folder", Application.persistentDataPath, null );
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void SaveButton()
    {
        Debug.Log("Save Button Pressed");
        FileBrowser.SetDefaultFilter(".hds");
        // FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
        StartCoroutine( ShowSaveDanceDataCoroutine() );
        // string json = JsonUtility.ToJson( m_danceData , true);
    }
    public void LoadButton()
    {
        Debug.Log("Load Button Pressed");
        FileBrowser.SetDefaultFilter(".hds");
        // FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
        StartCoroutine( ShowLoadDanceDataCoroutine() );
        // string json = JsonUtility.ToJson( m_danceData , true);
    }

    IEnumerator ShowSaveDanceDataCoroutine()
	{
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Holo Dance Studio Data", ".hds" ) );
        FileBrowser.SetDefaultFilter(".hds");
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForSaveDialog( FileBrowser.PickMode.Files , false, null, "HoloDanceData.hds", "Save Holo Dance Studio Data", "Save" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
			{
				Debug.Log( FileBrowser.Result[i] );
                SaveDataOn( FileBrowser.Result[i] );
				// StartCoroutine( saveda( FileBrowser.Result[i] ) );
			}

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			// byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );
		}
	}

    private void SaveDataOn( string p_path )
    {
        _DANCEDATA = m_danceRecorder.ExportDanceData();
        string json = JsonUtility.ToJson( _DANCEDATA , true);

        #if UNITY_ANDROID && !UNITY_EDITOR
        string nPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(p_path) );
        if( p_path != nPath )
        {
            NotificationScreen.Instance.ShowWindow( $"Due to Android OS limitations,\nyour file can only be saved in\n\"{Application.persistentDataPath}\"" );
        }
        p_path = nPath;
        // Debug.Log("Saving to: " + p_path);
        #endif
        FileBrowserHelpers.WriteTextToFile( p_path , json );

    }

    IEnumerator ShowLoadDanceDataCoroutine()
	{
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Holo Dance Studio Data", ".hds" ) );
        FileBrowser.SetDefaultFilter(".hds");
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files , false, null, "HoloDanceData.hds", "Choose Holo Dance Studio Data", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );
        yield return new WaitForSeconds(0.1f);
        NotificationScreen.Instance.ShowWindow("Loading Holo Dance Studio File");
        yield return new WaitForSeconds(0.1f);

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
			{
				Debug.Log( FileBrowser.Result[i] );
                LoadDataOn( FileBrowser.Result[i] );
				// StartCoroutine( saveda( FileBrowser.Result[i] ) );
			}

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			// byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );
		}
	}
    private void LoadDataOn( string p_path )
    {
        // #if UNITY_ANDROID
        // p_path = "file://"+p_path;
        // #endif
        DanceData dData = JsonUtility.FromJson<DanceData>( FileBrowserHelpers.ReadTextFromFile( p_path ) );
        m_danceRecorder.ClearTrack();
        m_danceRecorder.ImportDanceData( dData );
        _DANCEDATA = dData;
        // string json = JsonUtility.ToJson( m_danceRecorder.ExportDanceData() , true);
        // #if UNITY_ANDROID
        // p_path = "file://"+p_path;
        // #endif
        // FileBrowserHelpers.WriteTextToFile( p_path , json );

    }
}
