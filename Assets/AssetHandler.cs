using UnityEngine;
using UnityEditor;

public class EditorAssetHandling {
	public EditorAssetHandling(){}

	public void CreateDirIfNonExisting(string path){
		if(System.IO.Directory.Exists(path) ==false)
			System.IO.Directory.CreateDirectory(path);
	}

	public bool OverwriteDialog(){
		string title = "File already exists.";
		string message = "Overwrite?";
		string ok = "ok";
		string cancel = "nope";

		return EditorUtility.DisplayDialog(title, message, ok, cancel);
	}

	public bool CreateAtPathStorage(string path, string name, bool overwrite = false){
		AssetStorage storage = ScriptableObject.CreateInstance<AssetStorage>();
		name += ".asset";
		string fullPath = Application.dataPath;	//"C:/../Assets/"
		if (path != "") path = "/" +path +"/";
		else path = "/";
		fullPath += path;

		if( !overwrite && System.IO.File.Exists(fullPath +name) ){
			if(OverwriteDialog() ==false)
				return false;
		}
		CreateDirIfNonExisting(fullPath);

		AssetDatabase.CreateAsset(storage, "Assets" +path +name);
		AssetDatabase.Refresh();
		return true;
	}
}