using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameFramework{
[CreateAssetMenu(fileName = "AudioSettings",menuName = "Settings/AudioSettings")]
public class AudioSettings : Settings{

	public AudioMixer audioMixer;
	public AudioSave defaultSaveSettings;


	public static AudioSettings singleton;
	private static AudioSave currentSettings;

	[System.Serializable]
	public class AudioSave{
		public List<AudioParam>AudioParam = new List<AudioParam>();
	}
	
	[System.Serializable]
	public class AudioParam{
		public string name="Master";
		[Range(0,1)]
		public float value=1;
	}

	public override void Load(string s){	
		AudioSave save = null;
		if(!string.IsNullOrEmpty(s))
			save = JsonUtility.FromJson<AudioSave>(s);
		if(save != null && save.AudioParam.Count >0){
			currentSettings = save;
		}else{
			currentSettings = defaultSaveSettings;
		}
		foreach (var item in currentSettings.AudioParam) {
			audioMixer.SetFloat(item.name,db(item.value));
		}
		singleton = this;
	}

	public override string GetSave (){
		return JsonUtility.ToJson(currentSettings);
	}

	public override string GetID (){
		return "AudioSettings";
	}
	
	public static float db(float value) => value > 0.0f ? 20.0f * Mathf.Log10(value) : -80.0f;

	public static void Reset(){
		if(singleton){
			currentSettings = singleton.defaultSaveSettings;
			foreach (var item in currentSettings.AudioParam) {
				SetMixerVolume(item.value,item.name,singleton.audioMixer);
			}
		}
	}

	public static void SetAudioParam(string name,float volume){
		if(singleton){ 
			foreach (var item in currentSettings.AudioParam) {
				if(item.name.Equals(name)){
					SetMixerVolume(volume,name,singleton.audioMixer);
					item.value = volume;
					break;
				}
			}
		}
	}

	public static void SetMixerVolume(float value, string parameterName, AudioMixer mixer){
		mixer.SetFloat(parameterName,db(value));
	}

	public static float GetAudioParam(string name){
		if(singleton){ 
			foreach (var item in currentSettings.AudioParam) {
				if(item.name.Equals(name)){
					return item.value;
				}
			}
		}
		return 1;
	}
}
}
