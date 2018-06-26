using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

[AddComponentMenu("Localization/DropdownLanguages"),RequireComponent(typeof(Dropdown))]
public class DropdownLanguages :UIGlobalSettings {

	private Dropdown dropdown;

	void Awake(){
		dropdown = GetComponent<Dropdown>();
	}

	public override void Ini(){
		base.Ini();
		if(dropdown){
			LocalizationManager.instance.GetSupportedLanguages((SupportedLanguages languages)=>{
				SetLanguageNames(languages);
				CurrentLanguage();
				if(GameSettings.singleton)
					dropdown.onValueChanged.AddListener(GameSettings.SetLanguage);
				else{
					dropdown.onValueChanged.AddListener(LocalizationManager.SetLanguage);
				}
			});
		}
	}

	private void SetLanguageNames(SupportedLanguages languages){
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		foreach (var item in languages.items) {
			options.Add(new Dropdown.OptionData(item.languageName));
		}
		dropdown.options = options;
	}

	private void CurrentLanguage(){
		string ini = LocalizationManager.CurrentLanguage;
		for (int i = 0; i <dropdown.options.Count; i++) {
			if(dropdown.options[i].text == ini){
				dropdown.value = i;
				break;
			}
		}
	}

	public override void Reset(){	
		if(dropdown){
			LocalizationManager.instance.DefaultLanguage((string ini)=>{
				for (int i = 0; i <dropdown.options.Count; i++) {
					if(dropdown.options[i].text == ini){
						dropdown.value = i;
						break;
					}
				}
			});
		}
	}
}
