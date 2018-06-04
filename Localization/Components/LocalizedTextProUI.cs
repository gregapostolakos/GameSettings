using UnityEngine;
//using TMPro;

[AddComponentMenu("Localization/LocalizedTextProUI")]
public class LocalizedTextProUI : LocalizedTextBase {

    //private string id;
	//private TextMeshProUGUI text;

	void Reset(){
	//	text = GetComponent<TextMeshProUGUI>();
	}
	
	public override void Start(){
	/* 	text = GetComponent<TextMeshProUGUI>();
		if(text){
			id = text.text;
		} */
        base.Start();
	}
	
	public override void Verify() {
/*		string translateText = LocalizationManager.GetLocalizedValue(id).Replace("\\n","\n");
 		if(text){
			if(!string.IsNullOrEmpty(translateText))
				text.text = translateText;
			else{
				text.text = id;
			}
		} */
	}
}
