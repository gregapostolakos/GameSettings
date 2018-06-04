using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if STEAM
using System.ComponentModel;
using Steamworks;
#endif

namespace GameFramework{
public class SaveInfoBase{
	public virtual string SaveName(){
		return "Save";
	}
}

[Serializable]
public class SaveGame<T>where T:SaveInfoBase{

   	private static T instance = default(T);
	private static int slot;

	public static T Instance{
		get{
			if (instance==null)
				Load();
			return instance;
		}
	}

	public static void Save(){
		SetSlot(instance,slot);
	}

	public static void SetSlot(T instance, int slotId){
		if(IsSteam()){
			#if STEAM
			string json = JsonUtility.ToJson(instance);
	        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
			bool resp = SteamRemoteStorage.FileWrite(instance.SaveName()+""+slotId,bytes,bytes.Length);
			#endif
		}else{
			PlayerPrefs.SetString(instance.SaveName()+""+slotId,JsonUtility.ToJson(instance));
			PlayerPrefs.Save();
		}
		slot = slotId;
	}

	public static void Load(int slotId=0){
		instance = GetSlot(slotId);
		slot = slotId;
		if(instance == null){
			instance = Activator.CreateInstance<T>();
		}
	}

	public static T GetSlot(int slotId){
		
		if(IsSteam()){
			#if STEAM
			if(SteamRemoteStorage.FileExists(Activator.CreateInstance<T>().SaveName()+""+slotId)){
				byte[] bytes = new byte[1024*1024];
				int ret = SteamRemoteStorage.FileRead(Activator.CreateInstance<T>().SaveName()+""+slotId,bytes,bytes.Length);
				string data = System.Text.Encoding.UTF8.GetString(bytes, 0, ret);
				return JsonUtility.FromJson<T>(data);
			}
			#endif
		}else{
			string result = PlayerPrefs.GetString(Activator.CreateInstance<T>().SaveName()+""+slotId,"");	
			if(result != "") {
				return JsonUtility.FromJson<T>(result);
			}
		}
		return default(T);	
	}

	public static List<T> GetSlots(){
		List<T> slots= new List<T>();
		int id = 0;
		T slot = GetSlot(id);
		while(slot != null){
			slots.Add(slot);
			id++;
			slot = GetSlot(id);
		} 
		return slots;
	}

	private static bool IsSteam(){
		#if STEAM
		return true;
		#endif
		return false;
	}	
}
}