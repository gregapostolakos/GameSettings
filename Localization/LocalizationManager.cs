using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System;

public class DownloadFile{

	public static IEnumerator Download(string filePath, string result, Action action) {
        if (filePath.Contains("://")) {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        } else{
            result = System.IO.File.ReadAllText(filePath);
		}
		action.Invoke();
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
			DefaultLanguage(selectedLanguage,() => {});
		}
	}
	
	private static string selectedLanguage;

	public static string languagesDirectory{ 
		get { return Application.streamingAssetsPath +"/Languages";}
	}

	public static string languagesFile{ 
		get { return languagesDirectory+"/Languages.json";}
	} 

	public static string CurrentLanguage{
		get{
			return selectedLanguage;
		}
		set{ 
			if(selectedLanguage.Equals(value)) return;
			selectedLanguage = value;
			onUpdateLanguage.Invoke();
		}
	}

	public void GetSupportedLanguages(SupportedLanguages supportedLanguages, Action action){
		string jsonString="";
		StartCoroutine(DownloadFile.Download(languagesFile,jsonString, ()=>{
		  supportedLanguages = JsonUtility.FromJson<SupportedLanguages>(jsonString);
		  action.Invoke();
		}));
	}

	void GetText(string filePath, string key, string result, Action action){
		string jsonString="";
		LocalizedTexts localizedTexts=null;
		StartCoroutine(DownloadFile.Download(filePath,jsonString, ()=>{
		  localizedTexts = JsonUtility.FromJson<LocalizedTexts>(jsonString);
		  LocalizationItem item = localizedTexts.items.Find(o => o.key == key);
		  if(item != null){
			  result = item.value;
		  }
		  action.Invoke();
		}));
	}

	public void LoadLocalizedText(string language, string key, string result, Action action){
		SupportedLanguages supportedLanguages=null; 
		GetSupportedLanguages(supportedLanguages, ()=>{
			LanguageInfo info = supportedLanguages?.items.Find(o => o.languageName == language);
			if(info != null){
				GetText(languagesDirectory+"/"+info.fileName+".json",key, result, ()=>{
					action.Invoke();
				});
			}
		});
	}

	public void DefaultLanguage(string languageName, Action action){
		SupportedLanguages supportedLanguages= new SupportedLanguages();
		GetSupportedLanguages(supportedLanguages, ()=>{
			foreach (var item in supportedLanguages.items){
				if (item.systemLanguage == Application.systemLanguage){
					languageName = item.languageName;
					action.Invoke();
					return;
				}
			}
			if(string.IsNullOrEmpty(languageName) && supportedLanguages.items.Count>0){
				languageName = supportedLanguages.items[0].languageName;
			}
			action.Invoke();
		});
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod(){
        Debug.Log("Localization Manager Created");
		GameObject go = new GameObject("Localization Manager",typeof(LocalizationManager));
	}
}
