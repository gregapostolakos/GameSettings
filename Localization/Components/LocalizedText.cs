using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Localization/LocalizedText"),RequireComponent(typeof(Text))]
public class LocalizedText : LocalizedTextBase {

    public string key;
	private Text text;

	private bool keyText;
	private string defaultext;

	void Reset(){
		text = GetComponent<Text>();
	}
	
	public override void Start(){
		text = GetComponent<Text>();
		if(!text) return;

		if(string.IsNullOrEmpty(key)){
			key = text.text;
		}
		defaultext = text.text;
        base.Start();
	}
	
	public override void Verify() {
		string result = "";
		LocalizationManager.instance.LoadLocalizedText(LocalizationManager.CurrentLanguage, key, result, ()=>{
			if(!SetText(result)){
				string langName="";
				LocalizationManager.instance.DefaultLanguage(langName,()=>{
					LocalizationManager.instance.LoadLocalizedText(langName, key, result, ()=>{
						if(!SetText(result)){
							text.text = defaultext;
						}
					});
				});	
			}
		});
	}

	bool SetText(string t){
		if(!string.IsNullOrEmpty(t)){
			text.text = t.Replace("\\n","\n");
			return true;
		}
		return false;
	}
}
