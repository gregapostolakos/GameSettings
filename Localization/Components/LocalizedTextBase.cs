using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract  class LocalizedTextBase : MonoBehaviour {

    public virtual void Start(){
        LocalizationManager.onUpdateLanguage.AddListener(Verify);
        Verify();
    }

    public virtual void Verify(){}

    public virtual void OnDestroy(){
        LocalizationManager.onUpdateLanguage.RemoveListener(Verify);
    }
}
