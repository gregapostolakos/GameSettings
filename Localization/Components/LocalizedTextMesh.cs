using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Localization/LocalizedTextMesh"),RequireComponent(typeof(TextMesh))]
public class LocalizedTextMesh : LocalizedTextBase{

	public string key;
	private TextMesh text;

	private bool keyText;
	private string defaultext;

	void Reset(){
        text = GetComponent<TextMesh>();
	}
	
	public override void Start(){
		text = GetComponent<TextMesh>();
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
