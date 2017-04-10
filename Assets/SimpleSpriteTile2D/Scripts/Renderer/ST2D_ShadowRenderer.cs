using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[System.Serializable]
[RequireComponent (typeof(Transform))]
public class ST2D_ShadowRenderer : MonoBehaviour 
{
	public bool AddShadow = false;
	public Material forceReplacedMaterial;

	[HideInInspector]
	public bool ChangeSetting = false;
	

	void CheckDefaultMaterial(Transform target)
	{
		Material mat = target.GetComponent<Renderer>().sharedMaterial;
		if (mat.shader.name == "Sprites/Default" || mat.shader.name == "Sprites/Diffuse"){
			mat.shader = Shader.Find("ST2DShader/2DDiffuseShader");
		}

	}

	public void ForceReplaceChildMaterial(){
		var sprs = this.GetComponentsInChildren<SpriteRenderer>();
		foreach (var m_render in sprs){
			if (forceReplacedMaterial != null){
				m_render.material = forceReplacedMaterial;
			}
		}
		Debug.Log("已更新所有子物件的材质球");
	}

	void AddShadowInRenderer(Transform target){
		if (target.GetComponent<SpriteRenderer>() == null) return;
		CheckDefaultMaterial(target);
		target.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		target.GetComponent<Renderer>().receiveShadows = true;
        target.GetComponent<Renderer>().sharedMaterial.renderQueue = 2450;
	}

	void CloseShadowInRenderer(Transform target){
		if (target.GetComponent<SpriteRenderer>() == null) return;
		target.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        target.GetComponent<Renderer>().receiveShadows = false;
        target.GetComponent<Renderer>().sharedMaterial.renderQueue = 3000;
	}

	void UpdateAllChilds(){
		var sprs = this.GetComponentsInChildren<SpriteRenderer>();
		foreach (var m_render in sprs){
			if (AddShadow == true){
				AddShadowInRenderer(m_render.transform);
			}
			else{
				CloseShadowInRenderer(m_render.transform);
			}
		}

	}

	void OnEnable()
	{
		if (AddShadow == true){
			UpdateAllChilds();
			
		}
		else{
			UpdateAllChilds();
			Debug.Log("remove Shadow");
			
		}
	}
	void Update()
	{
		#if UNITY_EDITOR
		if (ChangeSetting == true){
			if (AddShadow == true){
				Debug.Log("Add Shadow");
				UpdateAllChilds();
			}
			else{
				Debug.Log("remove Shadow");
				UpdateAllChilds();
			}
			
		}
		#endif
	}
}
[CustomEditor(typeof(ST2D_ShadowRenderer)),CanEditMultipleObjects]
public class ST2D_ShadowRendererEditor:Editor
{
	
	void getProperty(){
		obj = new SerializedObject(target);
		AddShadow = obj.FindProperty("AddShadow");
		forceReplacedMaterial = obj.FindProperty("forceReplacedMaterial");
	}

	void Awake()
	{
		getProperty();
	}
	public override void OnInspectorGUI()
	{
		obj.Update();
		ST2D_ShadowRenderer scr = (ST2D_ShadowRenderer)target;
		var oldSet = scr.AddShadow;
		EditorGUILayout.PropertyField(AddShadow, new GUIContent("Add Shadow", "Add shadow for sprite"));
		EditorGUILayout.PropertyField(forceReplacedMaterial, new GUIContent("Force Material", "Replace all child mats"));
		
		obj.ApplyModifiedProperties();

		if (GUILayout.Button("Replace Child Materials")){
			scr.ForceReplaceChildMaterial();
		}

		if (scr.AddShadow == true && oldSet == false){
			scr.ChangeSetting = true;
		}
		else if (scr.AddShadow == false && oldSet == true){
			scr.ChangeSetting = true;
		}
		else{
			scr.ChangeSetting = false;
		}
        
		
	}
	private SerializedObject obj;

	public SerializedProperty AddShadow;

	public SerializedProperty forceReplacedMaterial;




	
}
