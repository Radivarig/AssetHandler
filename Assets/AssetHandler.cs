using UnityEngine;
using UnityEditor;

public class EditorAssetHandling {
	public EditorAssetHandling(){}

	public void CreateDirIfNonExisting(string path){
		if(System.IO.Directory.Exists(path) ==false)
			System.IO.Directory.CreateDirectory(path);
	}

	public AssetStorage CreateAtPathStorage(string path, string name){
		AssetStorage storage = ScriptableObject.CreateInstance<AssetStorage>();

		string fullPath = Application.dataPath;	//"C:/../Assets/"
		if (path != "") fullPath += "/" +path +"/";
		else fullPath += path; 
		CreateDirIfNonExisting(fullPath);

		string aPath = "Assets/";
		if (path != "") aPath += path +"/" +name +".asset";
		else aPath += name +".asset";
		AssetDatabase.CreateAsset(storage, aPath);
		AssetDatabase.Refresh();
		return storage;
	}
}