using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameSettings{
	
	public abstract class Settings : ScriptableObject{

		public virtual string GetSave(){
			return "";
		}

		public virtual void Load(string s){}

		public virtual string GetID(){
			return name;
		}
	}


	[CreateAssetMenu(fileName = "LanguageSettings",menuName = "GameSettings/Language Settings", order =1)]
	public class LanguageSettings : Settings{

		public LanguagemSettingsSave defaultSaveSettings;
		public LanguagemSettingsSave currentSettings;

		[System.Serializable]
		public class LanguagemSettingsSave{
			public string language;

			public LanguagemSettingsSave ShallowCopy(){
				return (LanguagemSettingsSave) this.MemberwiseClone();
			}
		}

		public static LanguageSettings singleton;

		public override void Load(string s){	
			singleton = this;
			LanguagemSettingsSave save = null;
			if(!string.IsNullOrEmpty(s))
				save = JsonUtility.FromJson<LanguagemSettingsSave>(s);
			if(save != null){
				currentSettings = save;
			}else{
				currentSettings = defaultSaveSettings.ShallowCopy();
			}
			if(!string.IsNullOrEmpty(currentSettings.language)){
				if(!LocalizationManager.instance) LocalizationManager.OnAfterSceneLoadRuntimeMethod();
				LocalizationManager.instance.GetSupportedLanguages((SupportedLanguages languages)=>{
					if(languages.items.Find(o => o.languageName.Equals(singleton.currentSettings.language)) != null){
						LocalizationManager.CurrentLanguage = singleton.currentSettings.language;
					}
				});
			}
				
		}

		public override string GetSave (){
			currentSettings.language = LocalizationManager.CurrentLanguage;
			return JsonUtility.ToJson(currentSettings);
		}

		public static void SetLanguage(int i){
			if(!singleton) return;
			LocalizationManager.instance.GetSupportedLanguages((SupportedLanguages languages)=>{
				LocalizationManager.CurrentLanguage = singleton.currentSettings.language = languages.items[i].languageName;
			});
		}
	}
}
