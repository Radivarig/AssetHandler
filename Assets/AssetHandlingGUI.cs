using UnityEngine;
using System.Collections;

public class AssetHandlingGUI : MonoBehaviour {
	float _wid = 0.2f;
	float _hei = 0.2f;

	void OnGUI(){
		Rect mainArea = new Rect(Screen.width *0.5f *(1 -_wid), Screen.height *0.5f *(1 -_hei), Screen.width *_wid, Screen.height *_hei);
		GUILayout.BeginArea(mainArea);
		{
			GUILayout.Button(".");
		}
		GUILayout.EndArea();
	}
}
