using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class DropdownLanguages :UIGlobalSettings {

	private Dropdown dropdown;

	void Awake(){
		dropdown = GetComponent<Dropdown>();
	}

	public override void Ini(){
		base.Ini();
		if(dropdown && GameSettings.singleton){
			dropdown.options = LanguageNames();
			string ini = LocalizationManager.CurrentLanguage;
			for (int i = 0; i <dropdown.options.Count; i++) {
				if(dropdown.options[i].text == ini){
					dropdown.value = i;
					break;
				}
			}
			dropdown.onValueChanged.AddListener(GameSettings.SetLanguage);
		}
	}

	private List<Dropdown.OptionData> LanguageNames(){
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		foreach (var item in LocalizationManager.SupportedLanguages) {
			options.Add(new Dropdown.OptionData(item.Value.name));
		}
		return options;
	}

	public override void Reset(){	
		if(dropdown){
			string ini = LocalizationManager.DefaultLanguage();
			for (int i = 0; i <dropdown.options.Count; i++) {
				if(dropdown.options[i].text == ini){
					dropdown.value = i;
					break;
				}
			}
		}
	}
}
