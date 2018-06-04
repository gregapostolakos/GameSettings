using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class SliderAudioVolume : UIGlobalSettings {

	public string paramName = "Music";
	private Slider slider;

	void Awake(){
		slider = GetComponent<Slider>();
	}

	public override void Ini(){
		base.Ini();
		if(slider && GameFramework.AudioSettings.singleton){
			slider.value = GameFramework.AudioSettings.GetAudioParam(paramName);
			slider.onValueChanged.AddListener(SetMusic);
		}
	}

	public void SetMusic(float value){
		GameFramework.AudioSettings.SetAudioParam(paramName,value);
	}

	public override void Reset(){	
		if(slider){
			slider.value = 1;
		}
	}
}
