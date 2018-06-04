using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class ButtonConfirm : MonoBehaviour {

	private Button button;

	void Start(){
		button = GetComponent<Button>();
		Ini();
	}

	public void Ini(){
		if(button && GameMaster.singleton){
			button.onClick.AddListener(GameMaster.Save);
		}
	}

}
