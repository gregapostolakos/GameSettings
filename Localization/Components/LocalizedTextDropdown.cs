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
			string result = "";
			string key = options[i].text;
			LocalizationManager.instance.LoadLocalizedText(LocalizationManager.CurrentLanguage, key, result, ()=>{
				if(!SetText(result,i)){
					string langName="";
					LocalizationManager.instance.DefaultLanguage(langName,()=>{
						LocalizationManager.instance.LoadLocalizedText(langName, key, result, ()=>{
							if(!SetText(result,i)){
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