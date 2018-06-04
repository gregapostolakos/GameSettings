using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Localization/LocalizedText"),RequireComponent(typeof(Text))]
public class LocalizedText : LocalizedTextBase {

    private string id;
	private Text text;

	void Reset(){
		text = GetComponent<Text>();
	}
	
	public override void Start(){
		text = GetComponent<Text>();
		if(text){
			id = text.text;
		}
        base.Start();
	}
	
	public override void Verify() {
		string translateText = LocalizationManager.GetLocalizedValue(id).Replace("\\n","\n");
		if(text){
			if(!string.IsNullOrEmpty(translateText))
				text.text = translateText;
			else{
				text.text = id;
			}
		}
	}
}
