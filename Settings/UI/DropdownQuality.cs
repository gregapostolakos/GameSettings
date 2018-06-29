using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;
using UnityEngine.Events;

[RequireComponent(typeof(Dropdown))]
public class DropdownQuality : UIGlobalSettings {

	public UnityEvent onLoaded;

	private Dropdown dropdown;

	void Awake(){
		dropdown = GetComponent<Dropdown>();
	}

	public override void Ini(){
		base.Ini();

		if(dropdown && VideoSettings.singleton){
			dropdown.options = GetQualityNames();
			dropdown.value = VideoSettings.GetQualityId();
			dropdown.onValueChanged.AddListener(VideoSettings.SetQuality);
			onLoaded.Invoke();
		}
	}

	public override void Reset(){	
		if(dropdown)
			dropdown.value = 0;
	}

	private List<Dropdown.OptionData> GetQualityNames(){
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		foreach (string item in VideoSettings.GetQualityNames()) {
			options.Add(new Dropdown.OptionData(item));
		}
		return options;
	}
}
