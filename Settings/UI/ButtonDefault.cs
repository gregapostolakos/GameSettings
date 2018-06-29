using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

public class ButtonDefault : MonoBehaviour {

	private Button button;

	void Start(){
		Ini();
	}

	public void Ini(){
		button = GetComponent<Button>();
		if(button && SettingsManager.singleton){
			button.onClick.AddListener(SettingsManager.Reset);
		}
	}
}
