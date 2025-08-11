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
	public class WorldScript : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private GameObject m_ChunkPrefab;

		public World World { get; private set; }
		public AsyncInstantiateOperation GenerationOperation { get; private set; }


		[Header("Options")]
		[SerializeField] private uint m_Size;
		[SerializeField] private uint m_Seed;

		public uint Size => m_Size;

		public void Awake()
		{
			Generate(m_Size, m_Seed);
		}

		public void Generate()
		{
			Generate(m_Size, m_Seed);
		}

		public void Generate(uint size, uint seed)
		{
			DestroyChildren();

			World = new World(size);

			ChunkScript.ID = 0;
			GenerationOperation = InstantiateAsync(m_ChunkPrefab, (int)(size * size), transform);

			GenerationOperation.completed += (AsyncOperation _) => World.Generate(new WorldGenerator(seed));
		}

		public void CancelGeneration()
		{
			if (GenerationOperation == null) return;

			GenerationOperation.Cancel();
		}

		private void DestroyChildren()
		{
			for (int i = transform.childCount - 1; i >= 0; --i)
			{
				DestroyImmediate(transform.GetChild(i).gameObject);
			}
		}
	}

	public class WorldGenerator : IWorldGenerator
	{
		private uint m_Seed;

		public WorldGenerator(uint seed)
		{
			m_Seed = seed;
		}

		public float GetHeight(uint x, uint z)
		{
			return 0f;
		}
	}
}
