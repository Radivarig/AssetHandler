using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class EditorAssetHandling {
	public EditorAssetHandling(){}

	public void CreateDirIfNonExisting(string path){
		if(Directory.Exists(path) ==false)
			Directory.CreateDirectory(path);
	}

	public bool OverwriteDialog(){
		string title = "File already exists.";
		string message = "Overwrite?";
		string ok = "ok";
		string cancel = "nope";

		return EditorUtility.DisplayDialog(title, message, ok, cancel);
	}

	public void CreateAtPathStorage(string path, string name, bool overwrite = false){
		AssetStorage storage = ScriptableObject.CreateInstance<AssetStorage>();
		name += ".asset";
		path = "/" +path +"/";
		string fullPath = Application.dataPath;	//"C:/../Assets"
		fullPath = PurgePathOfMultiSlashes(fullPath +path);
		
		if( !overwrite && File.Exists(fullPath +name))
			if(OverwriteDialog() ==false) return;

		CreateDirIfNonExisting(fullPath);
		path = PurgePathOfMultiSlashes("Assets/" +path);
		AssetDatabase.CreateAsset(storage, path +name);
	}

	public string PurgePathOfMultiSlashes(string path){
		string pattern = "\\/+";
		string replacement = "/";
		Regex rgx = new Regex(pattern);
		return rgx.Replace(path, replacement);
	}

	//TODO make this yield
	public List<string> GetAtPathFileNames(string path, string filter = "*"){
		FileInfo[] fileInfo = new FileInfo[0];
		List<string> fileNames = new List<string>();
		path = "/" +path +"/";
		string fullPath = PurgePathOfMultiSlashes(Application.dataPath +path);
		if(Directory.Exists(fullPath)){
			DirectoryInfo dir = new DirectoryInfo(fullPath);
			fileInfo = dir.GetFiles(filter);
			foreach(FileInfo f in fileInfo)
				fileNames.Add(f.Name);
		}
		return fileNames;
	}

	//TODO make this yield
	public List<string> ReplaceSubstringInList(List<string> list, string substring, string with){
		List<string> resultList = new List<string>();
		foreach (string str in list)
			resultList.Add(str.Replace(substring, with));
		return resultList;
	}

	public AssetStorage GetAtPathStorageStartsWith(string path, string startsWith){
		List<string> fileNames = GetAtPathFileNames(path, "*.asset");
		foreach(string fileName in ReplaceSubstringInList(fileNames, ".asset", ""))
			if(fileName.StartsWith(startsWith))
				return LoadAtPathStorage(path, fileName);
		return null;
	}

	public AssetStorage LoadAtPathStorage(string path, string name){
		string aPath = PurgePathOfMultiSlashes("Assets/" +path +"/" +name +".asset");
		return (AssetStorage)AssetDatabase.LoadAssetAtPath( aPath, typeof(AssetStorage));
	}

	public void DeleteStorage(AssetStorage storage){
		string path = AssetDatabase.GetAssetPath(storage);
		AssetDatabase.DeleteAsset(path);
	}

	public AssetStorage DuplicateStorage(AssetStorage storage, string path, string name){
		string aPath = PurgePathOfMultiSlashes("Assets/" +path +"/" +name +".asset");
		AssetDatabase.DeleteAsset(aPath);
		if(storage !=null){
			//EditorUtility.SetDirty(storage);
			//AssetDatabase.SaveAssets();
			AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(storage), aPath);
		}
		AssetDatabase.Refresh();
		return (AssetStorage)AssetDatabase.LoadAssetAtPath(aPath, typeof(AssetStorage));
	}
}
