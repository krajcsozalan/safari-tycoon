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

namespace SafariTycoon.Controllers
{
	[ExecuteAlways]
	public class ChunkScript : MonoBehaviour
	{
		private WorldScript m_WorldScript;

		public Chunk Chunk { get; private set; }

		[Header("Options")]
		[SerializeField] private uint m_Size;
		public static uint ID;

		public uint Size => m_Size;

		public void Awake()
		{
			m_WorldScript = GetComponentInParent<WorldScript>();

			uint x = ID % m_WorldScript.Size;
			uint z = ID / m_WorldScript.Size;

			transform.localPosition = new Vector3(x, 0f, z) * m_Size;
			name = $"Chunk ({x}, {z})";

			Chunk = new Chunk(m_Size, x, z);
			m_WorldScript.World.Chunks[x, z] = Chunk;

			++ID;
		}
	}
}
