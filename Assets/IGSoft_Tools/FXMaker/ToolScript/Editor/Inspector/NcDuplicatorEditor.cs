// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[CustomEditor(typeof(NcDuplicator))]

public class NcDuplicatorEditor : FXMakerEditor
{
	// Attribute ------------------------------------------------------------------------
	protected	NcDuplicator		m_Sel;

	// Property -------------------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
    void OnEnable()
    {
 		m_Sel = target as NcDuplicator;
 		m_UndoManager	= new FXMakerUndoManager(m_Sel, "NcDuplicator");
   }

    void OnDisable()
    {
    }

	public override void OnInspectorGUI()
	{
		AddScriptNameField(m_Sel);
		m_UndoManager.CheckUndo();
		// --------------------------------------------------------------
		bool bClickButton = false;
		EditorGUI.BeginChangeCheck();
		{
//			DrawDefaultInspector();
			m_Sel.m_fUserTag = EditorGUILayout.FloatField(GetCommonContent("m_fUserTag"), m_Sel.m_fUserTag);

			// wo mj begin
			m_Sel.m_meshFilter			= EditorGUILayout.ObjectField("Mesh", m_Sel.m_meshFilter, typeof(MeshFilter), true) as MeshFilter;
			if(m_Sel.m_meshFilter == null)
			{
				m_Sel.m_fDuplicateTime		= EditorGUILayout.FloatField	(GetHelpContent("m_fDuplicateTime")		, m_Sel.m_fDuplicateTime);
				m_Sel.m_nDuplicateCount		= EditorGUILayout.IntField		(GetHelpContent("m_nDuplicateCount")	, m_Sel.m_nDuplicateCount);
				m_Sel.m_fDuplicateLifeTime	= EditorGUILayout.FloatField	(GetHelpContent("m_fDuplicateLifeTime")	, m_Sel.m_fDuplicateLifeTime);
				m_Sel.m_RandomRange			= EditorGUILayout.Vector3Field	("m_RandomRange"						, m_Sel.m_RandomRange, null);
				m_Sel.m_StartPos			= EditorGUILayout.Vector3Field	("m_StartPos"							, m_Sel.m_StartPos, null);
				m_Sel.m_AddStartPos			= EditorGUILayout.Vector3Field	("m_AddStartPos"						, m_Sel.m_AddStartPos, null);
				m_Sel.m_AccumStartRot		= EditorGUILayout.Vector3Field	("m_AccumStartRot"						, m_Sel.m_AccumStartRot, null);
				m_Sel.m_AddAccumStartRot	= EditorGUILayout.Vector3Field	("m_AddAccumStartRot"					, m_Sel.m_AddAccumStartRot, null);
				m_Sel.m_scale				= EditorGUILayout.Vector3Field	("m_scale"								, m_Sel.m_scale, null);
				m_Sel.m_addScale			= EditorGUILayout.Vector3Field	("m_addScale"							, m_Sel.m_addScale, null);
				
				// NC_MODIFY BEGIN ~.~
				m_Sel.m_allAtOnce = EditorGUILayout.Toggle("AllAtOnce", m_Sel.m_allAtOnce);
				m_Sel.m_useSpawnRadius = EditorGUILayout.Toggle("UseSpawnRadius", m_Sel.m_useSpawnRadius);
				m_Sel.m_spawnRadius = EditorGUILayout.FloatField(GetHelpContent("m_spawnRadius"), m_Sel.m_spawnRadius);
				
				m_Sel.m_useSpawnDirection = EditorGUILayout.Toggle("UseSpawnDirection", m_Sel.m_useSpawnDirection);
				m_Sel.m_spawnDirectionAngle = EditorGUILayout.FloatField(GetHelpContent("SpawnDirectionAngle"), m_Sel.m_spawnDirectionAngle);
				m_Sel.m_spawnDirectionSpeed = EditorGUILayout.FloatField(GetHelpContent("SpawnDirectionSpeed"), m_Sel.m_spawnDirectionSpeed);
				
				SetMinValue(ref m_Sel.m_fDuplicateTime, 0f);
				SetMinValue(ref m_Sel.m_nDuplicateCount, 0);
				SetMinValue(ref m_Sel.m_fDuplicateLifeTime, 0);
			}
			else
			{

				m_Sel.m_spwanPositionMode	= (NcDuplicator.SpwanPositionMode)EditorGUILayout.EnumPopup("SpwanPositionMode", m_Sel.m_spwanPositionMode);
				m_Sel.m_fDuplicateTime		= EditorGUILayout.FloatField	(GetHelpContent("m_fDuplicateTime")		, m_Sel.m_fDuplicateTime);
				m_Sel.m_nDuplicateCount		= EditorGUILayout.IntField		(GetHelpContent("m_nDuplicateCount")	, m_Sel.m_nDuplicateCount);
				m_Sel.m_fDuplicateLifeTime	= EditorGUILayout.FloatField	(GetHelpContent("m_fDuplicateLifeTime")	, m_Sel.m_fDuplicateLifeTime);
				//m_Sel.m_RandomRange			= EditorGUILayout.Vector3Field	("m_RandomRange"						, m_Sel.m_RandomRange, null); 
				m_Sel.m_AddStartPos			= EditorGUILayout.Vector3Field	("m_AddStartPos"						, m_Sel.m_AddStartPos, null);
				m_Sel.m_AccumStartRot		= EditorGUILayout.Vector3Field	("m_AccumStartRot"						, m_Sel.m_AccumStartRot, null);
				m_Sel.m_AddAccumStartRot	= EditorGUILayout.Vector3Field	("m_AddAccumStartRot"					, m_Sel.m_AddAccumStartRot, null);
				m_Sel.m_scale				= EditorGUILayout.Vector3Field	("m_scale"								, m_Sel.m_scale, null);
				m_Sel.m_addScale			= EditorGUILayout.Vector3Field	("m_addScale"							, m_Sel.m_addScale, null);
				
				// NC_MODIFY BEGIN ~.~
				m_Sel.m_allAtOnce = EditorGUILayout.Toggle("AllAtOnce", m_Sel.m_allAtOnce); 
				m_Sel.m_random    = EditorGUILayout.Toggle("Random", m_Sel.m_random); 
				
				SetMinValue(ref m_Sel.m_fDuplicateTime, 0f);
				//SetMinValue(ref m_Sel.m_nDuplicateCount, 0);
				SetMinValue(ref m_Sel.m_fDuplicateLifeTime, 0);
			}
			// wo mj end

			// err check
			if (GetFXMakerMain())
				if (m_Sel.gameObject == GetFXMakerMain().GetOriginalEffectObject())
				{
					m_Sel.enabled = false;
// 					NgLayout.GUIColorBackup(Color.red);
// 					GUILayout.TextArea(GetHsScriptMessage("SCRIPT_ERROR_ROOT", ""), GUILayout.MaxHeight(80));
// 					NgLayout.GUIColorRestore();
				}
		}
		m_UndoManager.CheckDirty();
		// --------------------------------------------------------------
		if ((EditorGUI.EndChangeCheck() || bClickButton) && GetFXMakerMain())
			OnEditComponent();
		// ---------------------------------------------------------------------
		if (GUI.tooltip != "")
			m_LastTooltip	= GUI.tooltip;
		HelpBox(m_LastTooltip);
	}

	// ----------------------------------------------------------------------------------
	// ----------------------------------------------------------------------------------
	protected GUIContent GetHelpContent(string tooltip)
	{
		string caption	= tooltip;
		string text		= FXMakerTooltip.GetHsEditor_NcDuplicator(tooltip);
		return GetHelpContent(caption, text);
	}

	protected override void HelpBox(string caption)
	{
		string	str	= caption;
		if (caption == "" || caption == "Script")
			str = FXMakerTooltip.GetHsEditor_NcDuplicator("");
		base.HelpBox(str);
	}
}
