/* safari-tycoon - An open source safari tycoon game made in Unity.
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
using UnityEngine;
using UnityEditor;

namespace SafariTycoon.Controllers
{
	[CustomEditor(typeof(WorldScript))]
	public class WorldInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			WorldScript worldScript = target as WorldScript;

			GUILayout.Space(18);
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Generate"))
			{
				worldScript.Generate();
				worldScript.GenerationOperation.completed += (AsyncOperation _) => Repaint();
			}

			if (worldScript.GenerationOperation != null && !worldScript.GenerationOperation.isDone)
			{
				if (GUILayout.Button("Cancel"))
				{
					worldScript.CancelGeneration();	
				}
			}

			GUILayout.EndHorizontal();

			if (worldScript.GenerationOperation != null && worldScript.GenerationOperation.progress < 1f)
			{
				EditorGUI.ProgressBar(GUILayoutUtility.GetRect(0, 18), worldScript.GenerationOperation.progress, "Generation progress");
			}
		}
	}
}
