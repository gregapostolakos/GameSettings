using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

[RequireComponent(typeof(Dropdown))]
public class DropdownResolutions :UIGlobalSettings {

	private Dropdown dropdown;

	void Awake(){
		dropdown = GetComponent<Dropdown>();
	}

	public override void Ini(){
		base.Ini();
		if(dropdown && VideoSettings.singleton){
			dropdown.options = GetResolutionsNames();
			dropdown.value =  VideoSettings.GetResolutionId();
			dropdown.onValueChanged.AddListener(VideoSettings.SetResolution);
		}
	}

	public override void Reset(){	
		if(dropdown && VideoSettings.singleton){
			int i = VideoSettings.ResetResolution();
			if(i>=0)
				dropdown.value = i;
			else
				dropdown.value = dropdown.options.Count -1;
		}
	}

	public static List<Dropdown.OptionData> GetResolutionsNames(){
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		foreach (Resolution item in VideoSettings.GetResolutions) {
			options.Add(new Dropdown.OptionData(item.width + "x" + item.height));
		}
		return options;
	}

}
