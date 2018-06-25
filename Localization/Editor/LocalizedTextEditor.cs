using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

public class LocalizedTextEditor : EditorWindow{
    public LocalizedTexts localizationData;
	public SupportedLanguages supportedLanguages;
	
	private Vector2 scrollpos;

    [MenuItem ("Window/Localization Editor")]
    static void Init(){
        EditorWindow.GetWindow (typeof(LocalizedTextEditor)).Show ();
    }

    private void OnGUI(){

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		scrollpos = EditorGUILayout.BeginScrollView(scrollpos,false,false);
		OnGUILanguages();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	
		EditorGUILayout.LabelField("Texts",EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
        if (localizationData != null) {
			
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty serializedProperty = serializedObject.FindProperty ("localizationData");
            EditorGUILayout.PropertyField (serializedProperty, true);
            serializedObject.ApplyModifiedProperties();
			if (GUILayout.Button ("Save texts")){
				SaveTextsData ();
			}
			
        }
		if (GUILayout.Button ("Load texts")) {
			LoadTextsData ();
		}
		
		if (GUILayout.Button ("Create new data")) {
			CreateNewData ();
		}
		EditorGUILayout.EndScrollView();
    }

    private void LoadTextsData(){
		CreateDirectory();
        string filePath = EditorUtility.OpenFilePanel ("Select localization data file", LocalizationManager.languagesDirectory, "json");

        if (!string.IsNullOrEmpty (filePath)) {
			CreateNewData ();
			string jsonString = File.ReadAllText(filePath);
			localizationData = JsonUtility.FromJson<LocalizedTexts>(jsonString);
        }	
    }

    private void SaveTextsData(){
		CreateDirectory();
        string filePath = EditorUtility.SaveFilePanel ("Save localization data file", LocalizationManager.languagesDirectory, "", "json");
		if (string.IsNullOrEmpty(filePath)) return;
		string json = JsonUtility.ToJson(localizationData,true);
		Debug.Log(json);
	    File.WriteAllText(filePath, json);
		AssetDatabase.Refresh();
    }

    private void CreateNewData(){
        localizationData = new LocalizedTexts();
    }

	private void CreateDirectory(){
		if(Directory.Exists(LocalizationManager.languagesDirectory)) return;
		Directory.CreateDirectory(LocalizationManager.languagesDirectory);
		AssetDatabase.Refresh();
	}

	private void OnGUILanguages(){
		EditorGUILayout.LabelField("Languages",EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
        if (supportedLanguages != null) {
			
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty serializedProperty = serializedObject.FindProperty ("supportedLanguages");
            EditorGUILayout.PropertyField (serializedProperty, true);
            serializedObject.ApplyModifiedProperties();
			if (GUILayout.Button ("Save Languages")){
				SaveLanguagesData();
			}	
        }
		if (GUILayout.Button ("Load Languages")) {
			LoadLanguagesData();
		}
		if (GUILayout.Button ("Create new laguage list")) {
			CreateNewLanguagesData();
		}
    }

	private void SaveLanguagesData(){
		CreateDirectory();
		string json = JsonUtility.ToJson(supportedLanguages,true);
		Debug.Log(json);
	    File.WriteAllText(LocalizationManager.languagesFile, json);
		AssetDatabase.Refresh();
    }

	private void LoadLanguagesData(){
		CreateDirectory();
        string filePath = LocalizationManager.languagesFile;
        if (File.Exists(filePath)) {
			CreateNewData();
			string jsonString = File.ReadAllText(filePath);
			supportedLanguages = JsonUtility.FromJson<SupportedLanguages>(jsonString);
        }	
    }

	private void CreateNewLanguagesData(){
        supportedLanguages = new SupportedLanguages();
    }

}
