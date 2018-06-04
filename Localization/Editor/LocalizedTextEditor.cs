using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

public class LocalizedTextEditor : EditorWindow{
    public LocalizationData localizationData;
	
	private Vector2 scrollpos;

    [MenuItem ("Window/Localized Text Editor")]
    static void Init(){
        EditorWindow.GetWindow (typeof(LocalizedTextEditor)).Show ();
    }

    private void OnGUI(){
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		scrollpos = EditorGUILayout.BeginScrollView(scrollpos,false,false);
        if (localizationData != null) {
			
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty serializedProperty = serializedObject.FindProperty ("localizationData");
            EditorGUILayout.PropertyField (serializedProperty, true);
            serializedObject.ApplyModifiedProperties();
			if (GUILayout.Button ("Save data")){
				SaveGameData ();
			}
			
        }
		if (GUILayout.Button ("Load data")) {
			LoadGameData ();
		}
		
		if (GUILayout.Button ("Create new data")) {
			CreateNewData ();
		}

		EditorGUILayout.EndScrollView();
    }

    private void LoadGameData(){
		CreateDirectory();
        string filePath = EditorUtility.OpenFilePanel ("Select localization data file", LocalizationManager.languagesDirectory, "json");

        if (!string.IsNullOrEmpty (filePath)) {
			CreateNewData ();
			string jsonString = File.ReadAllText(filePath);
			localizationData = JsonUtility.FromJson<LocalizationData>(jsonString);
        }	
    }

    private void SaveGameData(){
		CreateDirectory();
        string filePath = EditorUtility.SaveFilePanel ("Save localization data file", LocalizationManager.languagesDirectory, "", "json");
		if (string.IsNullOrEmpty(filePath)) return;
		string json = JsonUtility.ToJson(localizationData,true);
		Debug.Log(json);
	    File.WriteAllText(filePath, json);
    }

    private void CreateNewData(){
        localizationData = new LocalizationData ();
    }

	private void CreateDirectory(){
		if(Directory.Exists(LocalizationManager.languagesDirectory)) return;
		Directory.CreateDirectory(LocalizationManager.languagesDirectory);
		AssetDatabase.Refresh();
	}

}
