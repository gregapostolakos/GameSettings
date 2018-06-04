using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class LocalizationData {
	public string languageName = "English";
	public SystemLanguage systemLanguage = SystemLanguage.Unknown;
	public List<LocalizationItem> items = new List<LocalizationItem>();
}

[System.Serializable]
public class LocalizationItem{
	[TextArea(2,50)]
	public string key;
	[TextArea(2,50)]
	public string value;
}

public static class LocalizationManager{

    public static UnityEvent onUpdateLanguage = new UnityEvent();

	private static Dictionary<string,LanguageInfo> supportedLanguages = new Dictionary<string, LanguageInfo>();
	private static Dictionary<string,string> localizedText = new Dictionary<string, string>();
	private static string loadedLanguage, firstLanguage, selectedLanguage;

	[System.Serializable]
	public class LanguageInfo{
		public string name;
		public string dataPath;
	
		public LanguageInfo(string n, string d){
			name = n;
			dataPath = d;
		}
	}
	
	public static string LoadedLanguage{
		get{ return loadedLanguage;}
	}
	public static Dictionary<string,LanguageInfo> SupportedLanguages{
		get{ LoadSupportedLanguages(); return supportedLanguages;}
	}
	public static string languagesDirectory{ 
		get { return Application.streamingAssetsPath +"/Languages";}
	} 

	public static string CurrentLanguage{
		get{
			if(!string.IsNullOrEmpty(selectedLanguage)){
				return selectedLanguage;
			}
            LoadSupportedLanguages();
			string dl = DefaultLanguage();
			if(!string.IsNullOrEmpty(dl)){
				selectedLanguage = dl;
			}else{
				selectedLanguage = "Unknown";
			}
			return selectedLanguage;
		}
		set{ selectedLanguage = value;
             onUpdateLanguage.Invoke();
		}
	}		

	public static string GetLocalizedValue(string id){			
		if(loadedLanguage != CurrentLanguage){
			LoadLocalizedTexts(CurrentLanguage);
		}
		if(localizedText.ContainsKey(id) && !string.IsNullOrEmpty(localizedText[id])){
			return localizedText[id];
		}
		return "";	
	}

	static void LoadLocalizedTexts(string language){
		if(!SupportedLanguages.ContainsKey(language)) return;
		Debug.Log("Load: "+language);
		string datapath = SupportedLanguages[language].dataPath;
		if(!File.Exists(datapath)) return;
		
		string jsonString = File.ReadAllText(datapath);
		LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(jsonString);
		loadedLanguage = language; 

		foreach (var item in localizationData.items){
			if(!localizedText.ContainsKey(item.key))			
				localizedText.Add(item.key,item.value);
			else
				localizedText[item.key] = item.value;
		}

		//string[] lines= File.ReadAllLines(datapath);
		/* for (int i = 0; i+1 < lines.Length; i+=2) {
			if(string.IsNullOrEmpty(lines[i]) || string.IsNullOrEmpty(lines[i+1])
				|| lines[i][0] == '#'|| lines[i+1][0] == '#'){
				i--;
				continue;
			}

			string id = lines[i];
			string text = lines[i+1];
			if(!localizedText.ContainsKey(id))			
				localizedText.Add(id,text);
			else
				localizedText[id] = text;
		} */
	}

	static void LoadSupportedLanguages(){	
		string[] files = Directory.GetFiles(languagesDirectory, @"*.json", SearchOption.TopDirectoryOnly);
		if(supportedLanguages.Count> 0 || files.Length<=0)
			return;	
		supportedLanguages.Clear();
		for (int i = 0; i < files.Length; i++) {
			string name = Path.GetFileNameWithoutExtension(files[i]);
			string path = files[i];
			LanguageInfo info = new LanguageInfo(name,path);
			if(string.IsNullOrEmpty(firstLanguage)) firstLanguage = name;
			supportedLanguages.Add(info.name,info);
		}
	}

	public static string DefaultLanguage(){
        //System Language
        foreach (var item in SupportedLanguages){
            if (item.Key == Application.systemLanguage.ToString()){
                return item.Key;
            }
        }
        // First Language in SupportedLanguages
		if(!string.IsNullOrEmpty(firstLanguage)){
			return firstLanguage;
		}
		return "";
	}
}
