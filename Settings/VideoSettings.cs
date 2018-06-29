using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameSettings{
	
public enum QualityVideoSettings{
	Off,
	Low,
	Medium,
	High
}

[CreateAssetMenu(fileName = "VideoSettings",menuName = "GameSettings/Video Settings", order = 3)]
public class VideoSettings : Settings {

	public float WantedAspectRatio = 1.777778f;
	public int maxNumResolutions = 5;
	public VideoSave defaultSaveSettings;
	public VideoSave currentSettings;
	public UnityEvent onUpdateResolution = new UnityEvent();
	
	public static VideoSettings singleton;

	[System.Serializable]
	public class VideoSave{
		[Range(0,2)]
		public float brightness = 1f;
		[Range(0,2)]
		public float contrast= 1f;
		public bool HDR;
		public QualityVideoSettings SSAO = QualityVideoSettings.Medium;

		public VideoSave ShallowCopy(){
			return (VideoSave) this.MemberwiseClone();
		}
	}

	public override void Load(string s){	
		VideoSave save = null;
		if(!string.IsNullOrEmpty(s))
			save = JsonUtility.FromJson<VideoSave>(s);
		if(save != null){
			currentSettings = save;
		}else{
			currentSettings = defaultSaveSettings.ShallowCopy();
		}
		singleton = this;
	}

	public override string GetSave (){
		return JsonUtility.ToJson(currentSettings);
	}

	public static void SetQuality(int i){
		QualitySettings.SetQualityLevel(i, true);
	}
	
	public static int GetQualityId(){
		return QualitySettings.GetQualityLevel();
	}

	public static string[] GetQualityNames(){
		return QualitySettings.names;
	}

	public static void SetResolution(int i){
		Screen.SetResolution(GetResolutions[i].width, GetResolutions[i].height,GetFullScreen());
		if(SettingsManager.singleton && singleton)
			SettingsManager.singleton.StartCoroutine(singleton.CheckResolution());
	}

	public static int GetResolutionId() {
		for (int i = 0; i < GetResolutions.Length; i++) {
			if(GetResolutions[i].width == Screen.width && GetResolutions[i].height == Screen.height) {
				return i;
			}
		}
		return GetResolutions.Length -1; 	
	}

	public static Resolution[] GetResolutions{
		get{
			if(singleton && Screen.resolutions.Length > singleton.maxNumResolutions*2){
				Resolution[] resolutions = new Resolution[singleton.maxNumResolutions];
				int j=0; 
				for (int i = Screen.resolutions.Length - (singleton.maxNumResolutions*2); i < Screen.resolutions.Length; i+=2) {
					resolutions[j] = Screen.resolutions[i];
					j++;
				}
				return resolutions;
			}
			return Screen.resolutions;
		}	
	}

	public static int ResetResolution(){
		for (int i = 0; i < GetResolutions.Length; i++) {
			if(GetResolutions[i].width == Screen.currentResolution.width && Screen.currentResolution.height == Screen.height) {
				return i;
			}
		}
		return -1; 
	}

	public static void SetFullScreen(bool b){
		Screen.fullScreen = b;
		if(SettingsManager.singleton && singleton)
			SettingsManager.singleton.StartCoroutine(singleton.CheckResolution());
	}

	public static bool GetFullScreen() {
		return Screen.fullScreen;
   	}

	public static float GetAspectRatio(){
		if(!singleton) return 1.777778f;
		return singleton.WantedAspectRatio;
	}

	IEnumerator CheckResolution(){
		yield return new WaitForSeconds(0.3f);
		UpdateAspectRatio();
		yield return new WaitForSeconds(1f);
		UpdateAspectRatio();
		yield return new WaitForSeconds(1.5f);
		UpdateAspectRatio();
	}

	void UpdateAspectRatio(){
		onUpdateResolution.Invoke();
	}
}
}