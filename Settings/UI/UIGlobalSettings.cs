using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSettings;

public class UIGlobalSettings : MonoBehaviour {

	public virtual void Start () {
		Ini();
	}

	public virtual void OnDestroy(){
		RemoveListeners();
	}

	public virtual void Ini(){
		SettingsManager.ResetAddListener(Reset);
	}

	public virtual void RemoveListeners(){
		SettingsManager.ResetRemoveListener(Reset);
	}

	public virtual void Reset(){	
		
	}
}
