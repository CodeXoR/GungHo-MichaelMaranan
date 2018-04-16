using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DiffuseOutlineGUI : ShaderGUI {

	bool showDiffuseComponent = true, showOutlineComponent = true;

	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties) {
		Material mat = materialEditor.target as Material;

		Color diffuse = mat.color;
		Color outlineColor = mat.GetColor ("_OutlineColor");
		float outlineThickness = mat.GetFloat ("_OutlineThickness");

		EditorGUI.BeginChangeCheck();

		FontStyle origFontStyle = EditorStyles.label.fontStyle;
		Color origLabelColor = EditorStyles.label.normal.textColor;

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorStyles.label.normal.textColor = Color.cyan;
		showDiffuseComponent = EditorGUILayout.Toggle ("[ DIFFUSE ]", showDiffuseComponent, EditorStyles.toggle);
		EditorStyles.label.fontStyle = origFontStyle;
		EditorStyles.label.normal.textColor = origLabelColor;

		if (showDiffuseComponent) {
			diffuse = EditorGUILayout.ColorField ("Color", diffuse);
		}

		EditorStyles.label.fontStyle = FontStyle.Bold;
		EditorStyles.label.normal.textColor = Color.yellow;
		showOutlineComponent = EditorGUILayout.Toggle ("[ OUTLINE ]", showOutlineComponent, EditorStyles.toggle);
		EditorStyles.label.fontStyle = origFontStyle;
		EditorStyles.label.normal.textColor = origLabelColor;

		if (showOutlineComponent) {
			outlineColor = EditorGUILayout.ColorField ("Color", outlineColor);
			outlineThickness = EditorGUILayout.Slider ("Thickness", outlineThickness, 0, 99);
		}

		if (EditorGUI.EndChangeCheck()) {
			mat.color = diffuse;
			mat.SetColor ("_OutlineColor", outlineColor);
			mat.SetFloat ("_OutlineThickness", outlineThickness);
		}
	}
}
