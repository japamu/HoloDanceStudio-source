using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.Networking;

public class MusicLoader : MonoBehaviour
{
	// Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
	// Warning: FileBrowser can only show 1 dialog at a time
	public MusicPlayer m_musicPlayer;

	void Start()
	{
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
        #if UNITY_ANDROID
		    FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".mp3", ".ogg") );
		    FileBrowser.SetDefaultFilter( ".mp3" );
        #else
		    FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".wav", ".ogg" ) );
		    FileBrowser.SetDefaultFilter( ".wav" );
        #endif

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

	}

    public void OnMusicButtonPress()
    {
        Debug.Log("MusicButton Pressed");
		StartCoroutine( ShowLoadMusicCoroutine() );
    }

	IEnumerator ShowLoadMusicCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Load Music", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
			{
				Debug.Log( FileBrowser.Result[i] );
				StartCoroutine( GetAudioClip( FileBrowser.Result[i] ) );
			}

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			// byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );
		}
	}

	IEnumerator GetAudioClip( string p_path )
    {
		#if UNITY_ANDROID
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://"+p_path , AudioType.MPEG))
		#else
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(p_path , AudioType.WAV))
		#endif
		{
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
				((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
				// using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(p_path , AudioType.MPEG))
				AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                
				m_musicPlayer.SetAudioFile( myClip );
				m_musicPlayer.CompletedLoading();
                Debug.Log("Music Load Successful");
            }
        }
    }
}
