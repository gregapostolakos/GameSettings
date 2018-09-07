using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;

namespace GameSettings{

	public class SettingsManager: MonoBehaviour{

		[EditScriptable]
		public SettingsGroup saveSettings;
		
		public static SettingsManager singleton;
		private UnityEvent onReset = new UnityEvent();

		public void Start(){
			saveSettings = Resources.Load<SettingsGroup>("SettingsGroup");
			if(singleton || !saveSettings){
				Destroy(gameObject);
				return;
			}
			Load();
		}

		public void Load(){
			singleton = this;
			DontDestroyOnLoad(gameObject);
			saveSettings.Load();
		}

		public static void Save(){
			if(!singleton) return;
			singleton.saveSettings.Save();
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

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void CreateInstance(){
			new GameObject("SettingsManager",typeof(SettingsManager));
		}
	}
}