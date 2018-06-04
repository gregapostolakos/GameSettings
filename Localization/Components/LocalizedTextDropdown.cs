using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
			string translateText = LocalizationManager.GetLocalizedValue(options[i].text).Replace("\\n","\n");
			if(!string.IsNullOrEmpty(translateText))
				dropdown.options[i].text = translateText;
				if(dropdown.value == i){
					dropdown.captionText.text = translateText;
				}		
			else{
				dropdown.options[i].text = options[i].text;
				if(dropdown.value == i){
					dropdown.captionText.text = translateText;
				}
			}
		}
	}
}