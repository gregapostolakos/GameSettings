using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings{
	
	[CreateAssetMenu(fileName = "AudioSettings",menuName = "GameSettings/Audio Settings", order = 2)]
	public class AudioSettings : Settings{

		public AudioMixer audioMixer;
		public AudioSave defaultSaveSettings;
		public AudioSave currentSettings;

		public static AudioSettings singleton;

		[System.Serializable]
		public class AudioSave{
			public List<AudioParam>AudioParam = new List<AudioParam>();

			public AudioSave Copy(){
				AudioSave a = new AudioSave();
				foreach (var item in AudioParam){
					AudioParam ap = new AudioParam();
					ap.name = item.name;
					ap.value = item.value;
					a.AudioParam.Add(ap);
				}
				return a;
			}
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
				currentSettings = defaultSaveSettings.Copy();
			}
			foreach (var item in currentSettings.AudioParam) {
				audioMixer.SetFloat(item.name,db(item.value));
			}
			singleton = this;
		}

		public override string GetSave (){
			return JsonUtility.ToJson(currentSettings);
		}
		
		public static float db(float value) => value > 0.0f ? 20.0f * Mathf.Log10(value) : -80.0f;

		public static void ResetAudio(){
			if(singleton){
				singleton.currentSettings = singleton.defaultSaveSettings.Copy();
				foreach (var item in singleton.currentSettings.AudioParam) {
					SetMixerVolume(item.value,item.name,singleton.audioMixer);
				}
			}
		}

		public static void SetAudioParam(string name,float volume){
			if(singleton){ 
				foreach (var item in singleton.currentSettings.AudioParam) {
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
				foreach (var item in singleton.currentSettings.AudioParam) {
					if(item.name.Equals(name)){
						return item.value;
					}
				}
			}
			return 1;
		}
	}
}
