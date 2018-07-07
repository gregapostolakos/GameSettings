using UnityEngine;
using System.Collections.Generic;

namespace GameSettings{

    [CreateAssetMenu(fileName = "SettingsGroup", menuName = "GameSettings/Settings Group", order = 0)]
	public class SettingsGroup: ScriptableObject{

		public string saveName="Settings";
		public List<Settings> settings = new List<Settings>();

		[System.Serializable]
		public class SaveStrings{
			public List<SaveSettingsString> list = new List<SaveSettingsString>();
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

		public void Save(){
			Debug.Log("list");
			SaveStrings sav = new SaveStrings();
			foreach (var item in settings) {
				Debug.Log(item.GetID());
				SaveSettingsString saveSettingsString = new SaveSettingsString(item.GetID(),item.GetSave());
				sav.list.Add(saveSettingsString);
			}
			SaveGame<SaveStrings>.Save(sav,saveName);
		}

		public void Load(){
			SaveStrings sav = SaveGame<SaveStrings>.Load(saveName);
			for (int i = 0; i < settings.Count; i++) {
				SaveSettingsString s = sav.list.Find(o =>o.id.Equals(settings[i].GetID()));
				if(s == null){
					settings[i].Load(null);
				}else settings[i].Load(s.save);
			}
		}
	}

}