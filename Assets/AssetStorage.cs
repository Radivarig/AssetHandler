using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetStorage : ScriptableObject {
	public List<ListItem> items;

	//when script gets enabled
	public void OnEnable(){
		//hideFlags = HideFlags.HideAndDontSave;
		if (items ==null)
			items = new List<ListItem>();
	}
}