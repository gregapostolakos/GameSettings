using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Localization/LocalizedTextMesh"),RequireComponent(typeof(TextMesh))]
public class LocalizedTextMesh : LocalizedTextBase{

	private TextMesh text;
    private string id;
	private string translateText;

	void Reset(){
        text = GetComponent<TextMesh>();
	}
	
	public override void Start(){
        text = GetComponent<TextMesh>();
		if(text){
			id = text.text;
		}
        base.Start();
	}

    public override void Verify(){
		translateText = LocalizationManager.GetLocalizedValue(id).Replace("\\n","\n");
		if(text){
			text.text = translateText;
		}
	}
}
