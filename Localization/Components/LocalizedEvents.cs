using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Localization/LocalizedEvents")]
public class LocalizedEvents : LocalizedTextBase {
	
    public StringEvent onChangeLanguage;
    public Languages[] onSelectedLanguageEvents;

    private string currentlang;

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
   
    [System.Serializable]
    public class Languages {
        public string language;
        public UnityEvent onSelectedLanguage;
    }
	
    public override void Verify() {
        currentlang = LocalizationManager.CurrentLanguage;
	    onChangeLanguage.Invoke( currentlang);
       
        foreach (Languages l in onSelectedLanguageEvents){
            if (l.language.Equals(currentlang)) {
                l.onSelectedLanguage.Invoke();
            }
        }
    }
}
