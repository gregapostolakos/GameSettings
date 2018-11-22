using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if STEAM
using System.ComponentModel;
using Steamworks;
#endif

namespace GameSettings{

	public abstract class ScriptableSaveBase<T>: ScriptableObject{

		public string saveName="Save";
		public int slot;
		public bool loadFromEditor;
		[SerializeField]
		protected T save;

		[NonSerialized]
		private bool loaded;

		public T Instance{
			get{
				#if UNITY_EDITOR
				if(loadFromEditor && !loaded){
					loaded = true;
					LoadEditor();
				}
				#endif
				if (!loaded){
					save = Load();
					loaded = true;
				}
				return save;
			}
		}

		public virtual void Save(){
			SaveGame<T>.Save(save,saveName,slot);
		}

		public T Load(){
			return SaveGame<T>.Load(saveName,slot);
		}

		public virtual void LoadEditor(){}
	}

	[Serializable]
	public class SaveGame<T>{


		public static void Save(T instance, string saveName, int slot=0){	
			#if STEAM
			string json = JsonUtility.ToJson(instance);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
			bool resp = SteamRemoteStorage.FileWrite(saveName+""+slot,bytes,bytes.Length);
			#elif UNITY_EDITOR
			string path = Application.dataPath+"/"+saveName+""+slot+".json";
			File.WriteAllText(path, JsonUtility.ToJson(instance,true));
			#else
			PlayerPrefs.SetString(saveName+""+slot,JsonUtility.ToJson(instance));
			PlayerPrefs.Save();
			#endif
		}

		public static T Load(string saveName, int slot=0){
			#if STEAM
			if(SteamRemoteStorage.FileExists(saveName+""+slot)){
				byte[] bytes = new byte[1024*1024];
				int ret = SteamRemoteStorage.FileRead(saveName+""+slot,bytes,bytes.Length);
				string data = System.Text.Encoding.UTF8.GetString(bytes, 0, ret);
				return JsonUtility.FromJson<T>(data);
			}
			#elif UNITY_EDITOR
			string path = Application.dataPath+"/"+saveName+""+slot+".json";
			if(File.Exists(path)){
				string result = File.ReadAllText(path);
				return JsonUtility.FromJson<T>(result);
			}	
			#else
			string result = PlayerPrefs.GetString(saveName+""+slot,"");	
			if(!string.IsNullOrEmpty(result)) {
				return JsonUtility.FromJson<T>(result);
			}
			#endif
			return Activator.CreateInstance<T>();
		}

		public static List<T> GetSlots(string saveName){
			List<T> slots= new List<T>();
			int id = 0;
			T slot = Load(saveName, id);
			while(slot != null){
				slots.Add(slot);
				id++;
				slot = Load(saveName, id);;
			} 
			return slots;
		}	
	}
}