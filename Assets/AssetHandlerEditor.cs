using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetHandlerEditor: EditorWindow {
	Rect mainArea = new Rect(0.5f, 0.5f, 0.2f, 0.2f);

	string storageName = "storage name";
	string storagePath = "SavedStorage";

	public EditorAssetHandling h = new EditorAssetHandling();
	
	[MenuItem ("Window/Asset Handler")]
	static void Init () {
		AssetHandlerEditor  editor = (AssetHandlerEditor)EditorWindow.GetWindow (typeof (AssetHandlerEditor));
		editor.title = "Asset Handler";
	}

	void OnGUI(){
		Rect _mainArea = new Rect(Screen.width *mainArea.x *(1 -mainArea.width), Screen.height *mainArea.y *(1 -mainArea.height), Screen.width *mainArea.width, Screen.height *mainArea.height);

		GUILayout.BeginArea(_mainArea);
		{
			CreateAssetGUI();
		}
		GUILayout.EndArea();
	}

	void CreateAssetGUI(){
		GUILayout.Label("this...Assets/");
		storagePath = GUILayout.TextField(storagePath);
		storageName = GUILayout.TextField(storageName);
		if(GUILayout.Button("Create Asset")){
			if (storageName == "") Debug.Log("Enter a name for asset file.");
			else h.CreateAtPathStorage(storagePath, storageName);
		}
	}
}

