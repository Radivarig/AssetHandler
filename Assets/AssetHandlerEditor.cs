using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AssetHandlerEditor: EditorWindow {
	Rect mainArea = new Rect(0.5f, 0.2f, 0.3f, 0.2f);

	Rect menuArea = new Rect(0.5f, 0.05f, 0.35f, 0.15f);
	string[] menuItems = new string[] {"Create", "Select", "Edit"};
	public int selMenu = 0;

	string storagePath = "AssetHandler";
	string storageName = "StorageAsset";
	string currentPrefix = "_current_";

	public EditorAssetHandling h = new EditorAssetHandling();

	[MenuItem ("Window/Asset Handler")]
	static void Init () {
		AssetHandlerEditor  editor = (AssetHandlerEditor)EditorWindow.GetWindow (typeof (AssetHandlerEditor));
		editor.title = "Asset Handler";
	}

	void OnGUI(){
		//revert this at end of ongui
		TextAnchor tempFieldAnchor = GUI.skin.textField.alignment; GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
		TextAnchor tempLabelAnchor = GUI.skin.label.alignment; GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		Rect _menuArea = new Rect(Screen.width *menuArea.x *(1 -menuArea.width), Screen.height *menuArea.y *(1 -menuArea.height), Screen.width *menuArea.width, Screen.height *menuArea.height);
		GUILayout.BeginArea(_menuArea);
		{
			selMenu = GUILayout.SelectionGrid(selMenu, menuItems, menuItems.Length);
			storagePath = GUILayout.TextField(storagePath);
			GUILayout.Label(GetCurrentStorageName());
		}
		GUILayout.EndArea();

		Rect _mainArea = new Rect(Screen.width *mainArea.x *(1 -mainArea.width), Screen.height *mainArea.y *(1 -mainArea.height), Screen.width *mainArea.width, Screen.height *mainArea.height);
		GUILayout.BeginArea(_mainArea);
		{
			if (menuItems[selMenu] == "Create") CreateAssetGUI();
			if (menuItems[selMenu] == "Select") SelectAssetGUI();
			if (menuItems[selMenu] == "Edit") EditAssetGUI();
		}
		GUILayout.EndArea();

		GUI.skin.textField.alignment = tempFieldAnchor;
		GUI.skin.label.alignment = tempLabelAnchor;
		this.Repaint();
	}

	void SelectAssetGUI(){
		List<string> fileNames = h.GetAtPathFileNames(storagePath, "*.asset");
		foreach(string fileName in h.ReplaceSubstringInList(fileNames, ".asset", "")){
			if(fileName.StartsWith(currentPrefix)) continue;
			//load - set storage as current
			if(GUILayout.Button(fileName)){
				AssetStorage currStorage = h.GetAtPathStorageStartsWith(storagePath, currentPrefix);
				h.DeleteStorage(currStorage);
				h.CreateAtPathStorage(storagePath, currentPrefix +fileName, true);
			}
		}
	}
	
	void CreateAssetGUI(){
		GUILayout.Label("Assets/" +storagePath);
		storageName = GUILayout.TextField(storageName);
		if(GUILayout.Button("Create Asset")){
			if (storageName == "") Debug.Log("Enter a name for asset file.");
			else h.CreateAtPathStorage(storagePath, storageName);
		}
	}

	void EditAssetGUI(){
		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("Duplicate")){
				AssetStorage current = h.GetAtPathStorageStartsWith(storagePath, currentPrefix);
				string curr = GetCurrentStorageName();
				h.DuplicateStorage(current, storagePath, curr +"_cpy");
			}
			
			if(GUILayout.Button("Delete")){
				AssetStorage storage = h.GetAtPathStorage(storagePath, storageName);
				if (storage){
					if(EditorUtility.DisplayDialog("Delete?", "Confirm", "ok", "nope")){
						h.DeleteStorage(h.GetAtPathStorageStartsWith(storagePath, currentPrefix));
						h.DeleteStorage(storage);
					}
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	string GetCurrentStorageName(){
		List<string> fileNames = h.GetAtPathFileNames(storagePath, currentPrefix +"*.asset");
		string curr = "no storage loaded";
		if (fileNames.Count > 0){
			curr = fileNames[0];
			curr = curr.Replace(currentPrefix, "");
			curr = curr.Replace(".asset", "");
		}
		return curr;
	}
}
