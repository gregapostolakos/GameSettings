using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

public class ButtonConfirm : MonoBehaviour {

	private Button button;

	void Start(){
		button = GetComponent<Button>();
		Ini();
	}

	public void Ini(){
		if(button && SettingsManager.singleton){
			button.onClick.AddListener(SettingsManager.Save);
		}
	}

}
