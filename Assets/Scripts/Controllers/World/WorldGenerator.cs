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
	public class WorldGenerator : IWorldGenerator
	{
		private float m_Displacement;
		private WorldGenerationConfig m_Config;

		public WorldGenerator(uint seed, WorldGenerationConfig config)
		{
			Random.InitState((int)seed);
			m_Displacement = Random.value;

			m_Config = config;
		}

		public float GetHeight(uint x, uint z)
		{
			float height = 0f;
			foreach (WorldGenerationLayer layer in m_Config.Layers)
			{
				height += layer.weight * Mathf.PerlinNoise(x / layer.smoothness + m_Displacement, z / layer.smoothness + m_Displacement) + layer.bias;
			}

			return height;
		}
	}
}
