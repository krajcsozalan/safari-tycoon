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
using SafariTycoon.Controllers;

namespace SafariTycoon.Views
{
	[ExecuteAlways]
	[RequireComponent(typeof(MeshFilter))]
	public class ChunkScript : MonoBehaviour
	{
		private static uint m_ID = 0;
		private MeshFilter m_MeshFilter;

		public Chunk Chunk { get; private set; }

		public void OnDestroy()
		{
			m_ID = 0;
		}

		public void Awake()
		{
			m_MeshFilter = GetComponent<MeshFilter>();

			World world = GetComponentInParent<WorldScript>().World;
			uint x = m_ID % world.Size;
			uint z = m_ID / world.Size;

			Chunk = world.Chunks[x, z];

			name = $"Chunk ({x}, {z})";
			transform.localPosition = new Vector3(x, 0f, z) * Chunk.Size;

			++m_ID;

			GenerateMesh();
		}

		private void GenerateMesh()
		{
			Mesh mesh = new Mesh();
			mesh.name = "Chunk";

			(uint x, uint z)[] vertexOffsets = new (uint, uint)[]
			{
				(0, 0),
				(0, 1),
				(1, 0),
				(1, 0),
				(0, 1),
				(1, 1)
			};

			Vector3[] vertices = new Vector3[Chunk.Size * Chunk.Size * vertexOffsets.Length];
			for (uint i = 0; i < Chunk.Size; ++i)
			{
				for (uint j = 0; j < Chunk.Size; ++j)
				{
					uint baseVertexIndex = (j * Chunk.Size + i) * (uint)vertexOffsets.Length;
					for (uint k = 0; k < vertexOffsets.Length; ++k)
					{
						uint x = i + vertexOffsets[k].x;
						uint z = j + vertexOffsets[k].z;

						vertices[baseVertexIndex + k] = new Vector3(x, Chunk.HeightMap[x, z], z);
					}
				}
			}
			mesh.SetVertices(vertices);

			Vector3[] normals = new Vector3[Chunk.Size * Chunk.Size * vertexOffsets.Length];
			for (uint i = 0; i < Chunk.Size; ++i)
			{
				for (uint j = 0; j < Chunk.Size; ++j)
				{
					uint baseVertexIndex = (j * Chunk.Size + i) * (uint)vertexOffsets.Length;

					float dx1 = Chunk.HeightMap[i, j] - Chunk.HeightMap[i + 1, j];
					float dx2 = Chunk.HeightMap[i, j + 1] - Chunk.HeightMap[i + 1, j + 1];
					float dz1 = Chunk.HeightMap[i, j] - Chunk.HeightMap[i, j + 1];
					float dz2 = Chunk.HeightMap[i + 1, j] - Chunk.HeightMap[i + 1, j + 1];

					Vector3 n1 = new Vector3(dx1, 1f, dz1).normalized;
					Vector3 n2 = new Vector3(dx2, 1f, dz1).normalized;
					Vector3 n3 = new Vector3(dx1, 1f, dz2).normalized;
					Vector3 n4 = new Vector3(dx2, 1f, dz2).normalized;

					normals[baseVertexIndex + 0] = n1;
					normals[baseVertexIndex + 1] = n2;
					normals[baseVertexIndex + 2] = n3;
					normals[baseVertexIndex + 3] = n3;
					normals[baseVertexIndex + 4] = n2;
					normals[baseVertexIndex + 5] = n4;
				}
			}
			mesh.SetNormals(normals);

			int[] triangles = new int[Chunk.Size * Chunk.Size * vertexOffsets.Length];
			for (int i = 0; i < Chunk.Size * Chunk.Size * vertexOffsets.Length; ++i)
			{
				triangles[i] = i;
			}
			mesh.SetTriangles(triangles, 0);

			m_MeshFilter.sharedMesh = mesh;
		}
	}
}
