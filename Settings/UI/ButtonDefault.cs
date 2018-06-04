using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public class ButtonDefault : MonoBehaviour {

	private Button button;

	void Start(){
		Ini();
	}

	public void Ini(){
		button = GetComponent<Button>();
		if(button && GameMaster.singleton){
			button.onClick.AddListener(GameMaster.Reset);
		}
	}
}
