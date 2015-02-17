using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AssetHandlerEditor: EditorWindow {
	Rect mainArea = new Rect(0.5f, 0.5f, 0.3f, 0.7f);
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
		GUI.skin = (GUISkin)Resources.Load("DefaultGUISkin", typeof(GUISkin));

		float _x = Screen.width *(mainArea.x -mainArea.width*0.5f);
		float _y = Screen.height *(mainArea.y -mainArea.height*0.5f);
		float _w = Screen.width *mainArea.width;
		float _h = Screen.height *mainArea.height;
		Rect _mainArea = new Rect(_x, _y, _w, _h);
		GUI.Box(_mainArea, "");
		GUILayout.BeginArea(_mainArea);
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label("Folder", GUILayout.Width(50f));
				storagePath = GUILayout.TextField(storagePath);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label("Selected");
				GUILayout.Label(GetCurrentStorageName());
			}
			EditorGUILayout.EndHorizontal();

			selMenu = GUILayout.SelectionGrid(selMenu, menuItems, menuItems.Length);

			if (menuItems[selMenu] == "Create") CreateAssetGUI();
			if (menuItems[selMenu] == "Select") SelectAssetGUI();
			if (menuItems[selMenu] == "Edit") EditAssetGUI();
		}
		GUILayout.EndArea();

		GUI.skin.textField.alignment = tempFieldAnchor;
		this.Repaint();
	}

	void SelectAssetGUI(){
		List<string> fileNames = h.GetAtPathFileNames(storagePath, "*.asset");
		_scrollSelect = GUILayout.BeginScrollView(_scrollSelect);
		{
			fileNames = h.ReplaceSubstringInList(fileNames, ".asset", "");
			fileNames.Sort();
			foreach(string fileName in fileNames){
				Texture2D buttonTexture = GUI.skin.button.normal.background;
				if (fileName == GetCurrentStorageName())
					GUI.skin.button.normal.background = GUI.skin.button.onNormal.background;

				if(fileName.StartsWith(currentPrefix)) continue;
				//load - set storage as current
				
				if(GUILayout.Button(fileName))
					SelectStorage(fileName);

				GUI.skin.button.normal.background = buttonTexture;
			}
		}
		GUILayout.EndScrollView();
	}

	void SelectStorage(string storageName){
		AssetStorage current = h.GetAtPathStorageStartsWith(storagePath, currentPrefix);
		AssetStorage storage = h.GetAtPathStorage(storagePath, storageName);
		h.DeleteStorage(current);
		h.DuplicateStorage(storage, storagePath, currentPrefix +storageName);
	}

	void CreateAssetGUI(){
		GUILayout.BeginScrollView(Vector2.zero);
		GUILayout.Label("Assets/" +storagePath);
		storageName = GUILayout.TextField(storageName);
		if(GUILayout.Button("Create Asset")){
			if (storageName == "") Debug.Log("Enter a name for asset file.");
			else h.CreateAtPathStorage(storagePath, storageName);
		}
		GUILayout.EndScrollView();
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
				h.DuplicateStorage(current, storagePath, h.FirstAvailableStorageName(storagePath, curr));
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
			if(GUILayout.Button("Apply"))
				h.DuplicateStorage(current, storagePath, GetCurrentStorageName());

			if(GUILayout.Button("Revert"))
				SelectStorage(GetCurrentStorageName());
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
				GUILayout.BeginHorizontal();
				{
					if(GUILayout.Button("x", GUILayout.Width(20f))){
						current.items.Remove(item);
						DestroyImmediate(item, true);
						AssetDatabase.SaveAssets();
						AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(current));
						break;
					}
					GUILayout.Label(item.property1);
				}
				GUILayout.EndHorizontal();
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
