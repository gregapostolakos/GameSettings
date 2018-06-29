using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

[RequireComponent(typeof(Toggle))]
public class ToggleFullScreen : UIGlobalSettings {

	private Toggle toggle;

	void Awake(){
		toggle = GetComponent<Toggle>();
	}

	public override void Ini(){
		base.Ini();
		if(toggle && VideoSettings.singleton){
			toggle.isOn = VideoSettings.GetFullScreen();
			toggle.onValueChanged.AddListener(VideoSettings.SetFullScreen);
		}
	}

	public override void Reset(){	
		if(toggle)
			toggle.isOn = true;
	}

}
