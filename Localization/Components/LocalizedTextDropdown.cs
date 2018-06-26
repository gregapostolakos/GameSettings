using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Localization/LocalizedTextDropdown")]
public class LocalizedTextDropdown : LocalizedTextBase{

	public Dropdown dropdown;
    private Dropdown.OptionData [] options;

	void Reset(){
		dropdown = GetComponent<Dropdown>();
	}
	
	public override void Start(){
		dropdown = GetComponent<Dropdown>();
		if(dropdown){
			options = dropdown.options.ToArray();
		}
        base.Start();
	}

	public override void Verify() {
		for (int i = 0; i < options.Length; i++){
			string key = options[i].text;
			LocalizationManager.instance.LoadLocalizedText(LocalizationManager.CurrentLanguage, key, (string result)=>{
				if(!SetText(result,i)){
					LocalizationManager.instance.DefaultLanguage((string langName)=>{
						LocalizationManager.instance.LoadLocalizedText(langName, key,( string result2)=>{
							if(!SetText(result2,i)){
								dropdown.options[i].text = options[i].text;
								if(dropdown.value == i){
									dropdown.captionText.text = options[i].text;
								}
							}
						});
					});	
				}
			});
		}
	}

	bool SetText(string t, int i){
		if(!string.IsNullOrEmpty(t)){
			dropdown.options[i].text = t;
			if(dropdown.value == i){
				dropdown.captionText.text = t;
			}
			return true;
		}
		return false;
	}
}