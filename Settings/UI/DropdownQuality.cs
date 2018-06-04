using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

[RequireComponent(typeof(Dropdown))]
public class DropdownQuality : UIGlobalSettings {

	private Dropdown dropdown;

	void Awake(){
		dropdown = GetComponent<Dropdown>();
	}

	public override void Ini(){
		base.Ini();
		LocalizationManager.onUpdateLanguage.AddListener(ChangeLanguage);
		if(dropdown && VideoSettings.singleton){
			dropdown.options = GetQualityNames();
			dropdown.value = VideoSettings.GetQualityId();
			dropdown.onValueChanged.AddListener(VideoSettings.SetQuality);
		}
	}

	public override void RemoveListeners(){
		base.RemoveListeners();
		LocalizationManager.onUpdateLanguage.RemoveListener(ChangeLanguage);
	}

	public override void Reset(){	
		if(dropdown)
			dropdown.value = 0;
	}

	private List<Dropdown.OptionData> GetQualityNames(){
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		foreach (string item in VideoSettings.GetQualityNames()) {
			options.Add(new Dropdown.OptionData(LocalizationManager.GetLocalizedValue(item)));
		}
		return options;
	}

	void ChangeLanguage(){
		if(dropdown)
			dropdown.options = GetQualityNames();
	}
}
