using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;

namespace GameFramework{
[Serializable]
public class SaveConf:SaveInfoBase{
	
	public List<SaveSettingsString> saveSettings = new List<SaveSettingsString>();

	public override string SaveName(){
		return "Conf";
	}

	public string getSaveSettings(string id){
		foreach (var item in saveSettings) {
			if(item.id.Equals(id)) return item.save;
		}
		return null;
	}

	public void Save(List<Settings> settings){
		saveSettings.Clear();
		foreach (var item in settings) {
			SaveSettingsString save = new SaveSettingsString(item.GetID(),item.GetSave());
			saveSettings.Add(save);
		}
		SaveGame<SaveConf>.Save();
	}
}

[System.Serializable]
public class SaveSettingsString{
	public string id;
	public string save;

	public SaveSettingsString(string id,string save){
		this.id = id;
		this.save = save;
	}
}

public class GameMaster: MonoBehaviour{

	public List<Settings> settings = new List<Settings>();
	
	public static GameMaster singleton;
	private UnityEvent onReset = new UnityEvent();

	public SaveConf saveConf{
		get{
			return SaveGame<SaveConf>.Instance;
		}
	}

	public void Start(){
		if(singleton){ 
			Destroy(gameObject);
			return;
		}
		Load();
	}

	public void Load(){
		singleton = this;
		DontDestroyOnLoad(gameObject);
		for (int i = 0; i < settings.Count; i++) {
			settings[i].Load(saveConf.getSaveSettings(settings[i].GetID()));
		}
	}

	public static void Save(){
		if(!singleton) return;
		singleton.saveConf.Save(singleton.settings);
	}

	public static void Reset(){
		if(singleton)
			singleton.onReset.Invoke();	
	}

	public static void ResetAddListener(UnityAction reset){
		if(singleton){
			singleton.onReset.AddListener(reset);
		}
	}

	public static void ResetRemoveListener(UnityAction reset){
		if(singleton){
			singleton.onReset.RemoveListener(reset);
		}
	}
}
}