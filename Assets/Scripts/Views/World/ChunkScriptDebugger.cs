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

using SafariTycoon.Model;

namespace SafariTycoon.Views
{
	[ExecuteAlways]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(ChunkScript))]
	public class ChunkScriptDebugger : MonoBehaviour
	{
		private MeshFilter m_MeshFilter;
		private Chunk m_Chunk;

		[Header("Options")]
		[SerializeField] private Color[] m_NormalDebugColors;

		public void Awake()
		{
			m_MeshFilter = GetComponent<MeshFilter>();
			m_Chunk = GetComponent<ChunkScript>().Chunk;
		}

		public void OnDrawGizmosSelected()
		{
			if (m_Chunk == null) return;

			for (uint i = 0; i < m_Chunk.Size; i++)
			{
				for (uint j = 0; j < m_Chunk.Size; j++)
				{
					uint baseVertexIndex = (j * m_Chunk.Size + i) * (uint)m_NormalDebugColors.Length;
					for (uint k = 0; k < m_NormalDebugColors.Length; ++k)
					{
						Gizmos.color = m_NormalDebugColors[k];
						Vector3 vertex = m_MeshFilter.sharedMesh.vertices[baseVertexIndex + k];
						Vector3 normal = m_MeshFilter.sharedMesh.normals[baseVertexIndex + k];

						Gizmos.DrawLine(transform.localPosition + vertex, transform.localPosition + vertex + normal);
					}
				}
			}
		}
	}
}
