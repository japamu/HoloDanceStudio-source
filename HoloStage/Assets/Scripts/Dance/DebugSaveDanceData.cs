using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class DebugSaveDanceData : MonoBehaviour
{

    private DanceData m_danceData;
    // Start is called before the first frame update
    void Start()
    {
        m_danceData = new DanceData();
        for( int i = 0 ; i < 5 ; i++ )
        {
            SavedFollowData temp1 = new SavedFollowData();
            temp1.timestamp = i;
            temp1.pointerPositions.Add ( new Vector2( i , -i ) );

            SavedAnimationData temp2 = new SavedAnimationData();
            temp2.timestamp = i;
            temp2.animType = i%2;
            temp2.animIndex = i;
            m_danceData.FollowDatas.Add( temp1 );
            m_danceData.AnimationDatas.Add( temp2 );
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveButton()
    {
        Debug.Log("Save Button Pressed");
        FileBrowser.SetDefaultFilter(".hds");
        StartCoroutine( ShowSaveDanceDataCoroutine() );
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

    public void SaveDataOn( string p_path )
    {
        string json = JsonUtility.ToJson( m_danceData , true);
        #if UNITY_ANDROID
        p_path = "file://"+p_path;
        #endif
        FileBrowserHelpers.WriteTextToFile( p_path , json );

    }


}
