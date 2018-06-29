using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

public class SliderAudioVolume : UIGlobalSettings {

	public string paramName = "Music";
	private Slider slider;

	void Awake(){
		slider = GetComponent<Slider>();
	}

	public override void Ini(){
		base.Ini();
		if(slider && GameSettings.AudioSettings.singleton){
			slider.value = GameSettings.AudioSettings.GetAudioParam(paramName);
			slider.onValueChanged.AddListener(SetMusic);
		}
	}

	public void SetMusic(float value){
		GameSettings.AudioSettings.SetAudioParam(paramName,value);
	}

	public override void Reset(){	
		if(slider){
			slider.value = 1;
		}
	}
}
