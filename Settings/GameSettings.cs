using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameFramework{
public class Settings : ScriptableObject{

	public virtual string GetSave(){
		return "";
	}

	public virtual void Load(string s){}

	public virtual string GetID(){
		return "";
	}
}

[CreateAssetMenu(fileName = "GameSettings",menuName = "Settings/GameSettings")]
public class GameSettings : Settings {

	public GameSettingsSave defaultSaveSettings;

	public static GameSettings singleton;
	private static GameSettingsSave currentSettings;

	[System.Serializable]
	public class GameSettingsSave{
		public string language;
	}

	public override void Load(string s){	
		singleton = this;
		GameSettingsSave save = null;
		if(!string.IsNullOrEmpty(s))
			save = JsonUtility.FromJson<GameSettingsSave>(s);
		if(save != null){
			currentSettings = save;
		}else{
			currentSettings = defaultSaveSettings;
		}
		if(!string.IsNullOrEmpty(currentSettings.language))
			LocalizationManager.CurrentLanguage = currentSettings.language;
	}

	public override string GetSave (){
		currentSettings.language = LocalizationManager.CurrentLanguage;
		return JsonUtility.ToJson(currentSettings);
	}

	public override string GetID (){
		return "GameSettings";
	}

	public static void SetLanguage(int i){
		LocalizationManager.CurrentLanguage = currentSettings.language = LanguageNames()[i];
	}

	private static List<string> LanguageNames(){
		List<string> names = new List<string>();
		foreach (var item in LocalizationManager.SupportedLanguages) {
			names.Add(item.Value.name);
		}
		return names;
	}
}
}
