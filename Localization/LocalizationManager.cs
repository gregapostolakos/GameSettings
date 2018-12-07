using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System;
using Steamworks;

public class DownloadFile{

	public static IEnumerator Download(string filePath, Action<String> action) {
    	string result="";
		if (filePath.Contains("://")) {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        } else{
            result = System.IO.File.ReadAllText(filePath);
		}
		action.Invoke(result);
	}
}


[System.Serializable]
public class SupportedLanguages{
	public List<LanguageInfo> items = new List<LanguageInfo>();
}

[System.Serializable]
public class LanguageInfo{
	public string languageName = "English";
	public string fileName;
	public string steamAPIName = "english";
	public SystemLanguage systemLanguage = SystemLanguage.Unknown;
}

[System.Serializable]
public class LocalizedTexts{
	public List<LocalizationItem> items = new List<LocalizationItem>();
}

[System.Serializable]
public class LocalizationItem{
	[TextArea(2,50)]
	public string key;
	[TextArea(2,50)]
	public string value;
}

public class LocalizationManager: MonoBehaviour{

    public static UnityEvent onUpdateLanguage = new UnityEvent();
	public static LocalizationManager instance;

	private void Awake() {
		Ini();
	}

	private void Ini() {
		if(instance && instance != this){
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
		if(string.IsNullOrEmpty(selectedLanguage)){
			DefaultLanguage((string result) => {
				selectedLanguage = result;
			});
		}
	}
	
	public string selectedLanguage;

	public static string languagesDirectory{ 
		get { return Application.streamingAssetsPath +"/Languages";}
	}

	public static string languagesFile{ 
		get { return languagesDirectory+"/Languages.json";}
	} 

	public static string CurrentLanguage{
		get{
			return instance.selectedLanguage;
		}
		set{ 
			if(instance.selectedLanguage.Equals(value)) return;
			instance.selectedLanguage = value;
			onUpdateLanguage.Invoke();
		}
	}

	public void GetSupportedLanguages(Action<SupportedLanguages> action){
		if(!File.Exists(languagesFile)) return;
		StartCoroutine(DownloadFile.Download(languagesFile, (string jsonString)=>{
		  	SupportedLanguages supportedLanguages = JsonUtility.FromJson<SupportedLanguages>(jsonString);
		  	action.Invoke(supportedLanguages);
		}));
	}

	void GetText(string filePath, string key,Action<string> action){
		LocalizedTexts localizedTexts=null;
		StartCoroutine(DownloadFile.Download(filePath, (string jsonString)=>{
		  	localizedTexts = JsonUtility.FromJson<LocalizedTexts>(jsonString);
		  	LocalizationItem item = localizedTexts.items.Find(o => o.key == key);
			string result=""; 
		  	if(item != null){
				result = item.value;
		  	}
		  	action.Invoke(result);
		}));
	}

	public void LoadLocalizedText(string language, string key, Action<string> action){
		GetSupportedLanguages((SupportedLanguages supportedLanguages)=>{
			LanguageInfo info = supportedLanguages?.items.Find(o => o.languageName == language);
			if(info != null){
				GetText(languagesDirectory+"/"+info.fileName+".json",key, (string result)=>{
					action.Invoke(result);
				});
			}
		});
	}

	public void DefaultLanguage(Action<string> action){
		string languageName="";
		GetSupportedLanguages((SupportedLanguages supportedLanguages)=>{
#if STEAM
			if (SetSteamLanguage(action, supportedLanguages, ref languageName)) return;
#endif
			if (SetSysemLanguage(action, supportedLanguages, ref languageName)) return;
			SetFirstLanguageAvailable(action, languageName, supportedLanguages);
		});
	}

	private static void SetFirstLanguageAvailable(Action<string> action, string languageName,
		SupportedLanguages supportedLanguages)
	{
		if (string.IsNullOrEmpty(languageName) && supportedLanguages.items.Count > 0)
		{
			languageName = supportedLanguages.items[0].languageName;
		}

		action.Invoke(languageName);
	}

	private static bool SetSysemLanguage(Action<string> action, SupportedLanguages supportedLanguages, ref string languageName)
	{
		foreach (var item in supportedLanguages.items)
		{
			if (item.systemLanguage == Application.systemLanguage)
			{
				languageName = item.languageName;
				action.Invoke(languageName);
				return true;
			}
		}

		return false;
	}

#if STEAM
	private static bool SetSteamLanguage(Action<string> action, SupportedLanguages supportedLanguages, ref string languageName)
	{
		string steamLanguage = SteamApps.GetCurrentGameLanguage();
		foreach (var item in supportedLanguages.items)
		{
			if (item.steamAPIName == steamLanguage)
			{
				languageName = item.languageName;
				action.Invoke(languageName);
				return true;
			}
		}
		return false;
	}
#endif

	[ContextMenu("UpdateLanguage")]
	public void UpdateLanguage()
	{
		onUpdateLanguage.Invoke();
	}

	public static void SetLanguage(int i){
		instance.GetSupportedLanguages((SupportedLanguages languages)=>{
			CurrentLanguage = languages.items[i].languageName;
		});
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnAfterSceneLoadRuntimeMethod(){
        Debug.Log("Localization Manager Created");
		new GameObject("Localization Manager",typeof(LocalizationManager));
	}
}
