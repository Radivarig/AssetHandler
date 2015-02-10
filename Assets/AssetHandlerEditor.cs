using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetHandlerEditor: EditorWindow {
	float _wid = 0.2f;
	float _hei = 0.2f;
	string storageName = "";

	public EditorAssetHandling h = new EditorAssetHandling();

	[MenuItem ("Window/Asset Handler")]
	static void Init () {
		AssetHandlerEditor  editor = (AssetHandlerEditor)EditorWindow.GetWindow (typeof (AssetHandlerEditor));
		editor.title = "Asset Handler";
	}

	void OnGUI(){
		Rect mainArea = new Rect(Screen.width *0.5f *(1 -_wid), Screen.height *0.5f *(1 -_hei), Screen.width *_wid, Screen.height *_hei);
		GUILayout.BeginArea(mainArea);
		{
			if(GUILayout.Button("_")){
			}
		}
		GUILayout.EndArea();
	}
}
