using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WW.Waiters
{
	[CustomEditor(typeof(WW.Waiters.WaitController))]
	public class WaitControllerEditor : Editor
	{

		SerializedProperty sp_m_Script;

		private WaitController _target;
		private bool _showTrackedWaiters = false;
		private bool _showPendingWaiters = false;

		void OnEnable()
		{
			sp_m_Script = serializedObject.FindProperty("m_Script");

			_target = serializedObject.targetObject as WaitController;

			//to view realtime counts we want to repaint every frame
			EditorApplication.update += () => this.Repaint();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.Space();

			GUI.enabled = false;
			EditorGUILayout.PropertyField(sp_m_Script, true);
			GUI.enabled = true;

			_showTrackedWaiters = EditorGUILayout.Foldout(_showTrackedWaiters, string.Format("Tracked Waiters: {0}", _target.TrackedWaiters.Count));
			if(_showTrackedWaiters)
			{
				EditorGUI.indentLevel++;

				for(int i = 0; i < _target.TrackedWaiters.Count; i++)
				{
					if(_target.TrackedWaiters[i].ID != null && _target.TrackedWaiters[i].ID as MonoBehaviour != null)
					{
						MonoBehaviour mb = (_target.TrackedWaiters[i].ID as MonoBehaviour);

						if(GUI.Button(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect()), _target.TrackedWaiters[i].ID.ToString(), EditorStyles.boldLabel))
						{
							Selection.activeObject = mb.gameObject;
						}
					}
					else
					{
						EditorGUILayout.LabelField(_target.TrackedWaiters[i].ID == null ? "No ID" : _target.TrackedWaiters[i].ID.ToString());
					}
				}

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			_showPendingWaiters = EditorGUILayout.Foldout(_showPendingWaiters, string.Format("Pending Waiters: {0}", _target.PendingWaiters.Count));
			if(_showPendingWaiters)
			{
				EditorGUI.indentLevel++;

				for(int i = 0; i < _target.PendingWaiters.Count; i++)
				{
					string name = "";
					if(_target.PendingWaiters[i].ID == null)
						name = "no ID";
					else if(_target.PendingWaiters[i].ID.GetType() == typeof(MonoBehaviour))
						name = (_target.PendingWaiters[i].ID as MonoBehaviour).name;
					else
						name = _target.PendingWaiters[i].ID.ToString();

					EditorGUILayout.LabelField(name);
				}

				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
