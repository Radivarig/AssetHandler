using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetHandlerEditor: EditorWindow {
	Rect mainArea = new Rect(0.5f, 0.5f, 0.2f, 0.2f);

	Rect menuArea = new Rect(0.5f, 0.05f, 0.6f, 0.05f);
	string[] menuItems = new string[] {"Create", "Grid 2", "Grid 3", "Grid 4"};
	public int selMenu = 0;


	string storageName = "storage name";
	string storagePath = "SavedStorage";

	public EditorAssetHandling h = new EditorAssetHandling();

	[MenuItem ("Window/Asset Handler")]
	static void Init () {
		AssetHandlerEditor  editor = (AssetHandlerEditor)EditorWindow.GetWindow (typeof (AssetHandlerEditor));
		editor.title = "Asset Handler";
	}

	void OnGUI(){
		Rect _menuArea = new Rect(Screen.width *menuArea.x *(1 -menuArea.width), Screen.height *menuArea.y *(1 -menuArea.height), Screen.width *menuArea.width, Screen.height *menuArea.height);
		GUILayout.BeginArea(_menuArea);
		{
			selMenu = GUILayout.SelectionGrid(selMenu, menuItems, menuItems.Length);
		}
		GUILayout.EndArea();

		Rect _mainArea = new Rect(Screen.width *mainArea.x *(1 -mainArea.width), Screen.height *mainArea.y *(1 -mainArea.height), Screen.width *mainArea.width, Screen.height *mainArea.height);
		GUILayout.BeginArea(_mainArea);
		{
			if (menuItems[selMenu] == "Create") CreateAssetGUI();
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

