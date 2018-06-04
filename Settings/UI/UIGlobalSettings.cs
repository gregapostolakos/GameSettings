using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class UIGlobalSettings : MonoBehaviour {

	public virtual void Start () {
		Ini();
	}

	public virtual void OnDestroy(){
		RemoveListeners();
	}

	public virtual void Ini(){
		GameMaster.ResetAddListener(Reset);
	}

	public virtual void RemoveListeners(){
		GameMaster.ResetRemoveListener(Reset);
	}

	public virtual void Reset(){	
		
	}
}
