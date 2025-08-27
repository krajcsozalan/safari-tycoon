/*
 * safari-tycoon - An open-source tycoon game made with Unity
 * Copyright (C) 2025  krajcsozalan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;

using UnityEngine;
using UnityEditor;

namespace SafariTycoon.View
{
	[CustomEditor(typeof(WorldView))]
	public class WorldViewInspector : Editor
	{
		private uint m_WorldSize;
		private uint m_ChunkSize;

		public override void OnInspectorGUI()
		{
			GUIStyle headerStyle = new GUIStyle() { normal = new GUIStyleState() { textColor = new Color(0.8f, 0.8f, 0.8f) }, fontStyle = FontStyle.Bold };

			DrawDefaultInspector();

			GUILayout.Space(10);
			GUILayout.Label("Generation settings", headerStyle);

			m_WorldSize = UIntField("World size", m_WorldSize);
			m_ChunkSize = UIntField("Chunk size", m_ChunkSize);

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Generate"))
			{
				(target as WorldView).Generate(m_WorldSize, m_ChunkSize);
			}

			if (GUILayout.Button("Cancel"))
			{
				(target as WorldView).CancelGeneration();
			}

			GUILayout.EndHorizontal();
		}

		private uint UIntField(string label, uint value, params GUILayoutOption[] options)
		{
			int signedValue = Convert.ToInt32(value);
			signedValue = EditorGUILayout.IntField(label, signedValue, options);

			try
			{
				return Convert.ToUInt32(signedValue);
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}
}
