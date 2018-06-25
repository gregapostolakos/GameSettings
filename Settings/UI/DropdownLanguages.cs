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
		if(dropdown && GameSettings.singleton){
			SupportedLanguages languages=new SupportedLanguages();
			LocalizationManager.instance.GetSupportedLanguages(languages, ()=>{
				SetLanguageNames(languages);
				CurrentLanguage();
				dropdown.onValueChanged.AddListener(GameSettings.SetLanguage);
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
			string ini="";
			LocalizationManager.instance.DefaultLanguage(ini,()=>{
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
