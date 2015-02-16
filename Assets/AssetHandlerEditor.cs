using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AssetHandlerEditor: EditorWindow {
	Rect mainArea = new Rect(0.5f, 0.5f, 0.3f, 0.6f);

	Rect menuArea = new Rect(0.5f, 0.05f, 0.35f, 0.15f);
	string[] menuItems = new string[] {"Create", "Select", "Edit"};
	public int selMenu = 0;

	string storagePath = "AssetHandler";
	string storageName = "StorageAsset";
	string currentPrefix = "_current_";

	Vector2 _scrollSelect= new Vector2();
	Vector2 _scrollEdit = new Vector2();

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
		GUI.Box(_menuArea, "");
		GUILayout.BeginArea(_menuArea);
		{
			selMenu = GUILayout.SelectionGrid(selMenu, menuItems, menuItems.Length);
			storagePath = GUILayout.TextField(storagePath);
			GUILayout.Label(GetCurrentStorageName());
		}
		GUILayout.EndArea();

		Rect _mainArea = new Rect(Screen.width *mainArea.x *(1 -mainArea.width), Screen.height *mainArea.y *(1 -mainArea.height), Screen.width *mainArea.width, Screen.height *mainArea.height);
		GUI.Box(_mainArea, "");
		GUILayout.BeginArea(_mainArea);
		{
			if (menuItems[selMenu] == "Create") CreateAssetGUI();
			if (menuItems[selMenu] == "Select") SelectAssetGUI();
			if (menuItems[selMenu] == "Edit") EditAssetGUI();
		}
		GUILayout.EndArea();

		GUI.skin.textField.alignment = tempFieldAnchor;
		GUI.skin.label.alignment = tempLabelAnchor;
		//this.Repaint();
	}

	void SelectAssetGUI(){
		List<string> fileNames = h.GetAtPathFileNames(storagePath, "*.asset");
		_scrollSelect = GUILayout.BeginScrollView(_scrollSelect);
		{
			fileNames = h.ReplaceSubstringInList(fileNames, ".asset", "");
			fileNames.Sort();
			foreach(string fileName in fileNames){
				if(fileName.StartsWith(currentPrefix)) continue;
				//load - set storage as current
				if(GUILayout.Button(fileName))
					SelectStorage(fileName);
			}
		}
		GUILayout.EndScrollView();
	}

	void SelectStorage(string storageName){
		AssetStorage currStorage = h.GetAtPathStorageStartsWith(storagePath, currentPrefix);
		h.DeleteStorage(currStorage);
		h.CreateAtPathStorage(storagePath, currentPrefix +storageName, true);
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
		AssetStorage current = h.GetAtPathStorageStartsWith(storagePath, currentPrefix);
		AssetStorage storage = h.GetAtPathStorage(storagePath, GetCurrentStorageName());
		if (storage ==null) h.DeleteStorage(current);
		if (current ==null) return;

		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("Duplicate")){
				string curr = GetCurrentStorageName();
				h.DuplicateStorage(current, storagePath, curr +"_cpy");
			}
			
			if(GUILayout.Button("Delete")){
				if (storage){
					if(EditorUtility.DisplayDialog("Delete?", "Confirm", "ok", "nope")){
						h.DeleteStorage(storage);
						h.DeleteStorage(current);
					}
				}
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("Apply")){
			}

			if(GUILayout.Button("Revert")){
				SelectStorage(GetCurrentStorageName());
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Add new:");
		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("ListItem")){
				ListItem item = ScriptableObject.CreateInstance<ListItem>();
				current.items.Add(item);
				h.AddObjectToStorage(item, current);
			}

			if(GUILayout.Button("ListItemDerrived")){
				ListItem item = ScriptableObject.CreateInstance<ListItemDerrived>();
				current.items.Add(item);
				h.AddObjectToStorage(item, current);
			}
		}
		GUILayout.EndHorizontal();
		
		_scrollEdit = GUILayout.BeginScrollView(_scrollEdit);
		GUILayout.BeginVertical();
		{
			foreach (ListItem item in current.items){
				GUILayout.Label(item.property1);
			}
		}
		GUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
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
