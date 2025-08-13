/* safari-tycoon
 * Copyright (C) 2025  krajcsozalan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
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

using NUnit.Framework;

using SafariTycoon.Model;

namespace SafariTycoon.Tests
{
	public class ChunkTests
	{
		private static uint[] m_Sizes = { 0, 1, 2, 3, 5, 7, 8 };
		private static float[] m_Heights = { float.NaN, -1f, -0.5f, 0f, 0.5f, 1f, 1024f };

		[Test]
		public void ConstructorTests([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(size, size);

			foreach (Chunk chunk in world.Chunks)
			{
				Assert.That(chunk.Size, Is.EqualTo(size));
				Assert.That(chunk.HeightMap.GetLength(0), Is.EqualTo(size));
				Assert.That(chunk.HeightMap.GetLength(1), Is.EqualTo(size));
			}
		}

		private class WorldGenerator : IWorldGenerator
		{
			private bool m_Constant;
			private float m_Height;

			public WorldGenerator(bool constant, float height = 0f)
			{
				m_Constant = constant;
				m_Height = height;
			}

			public float GetHeight(uint x, uint z)
			{
				return m_Constant ? m_Height : (x + z);
			}
		}

		[Test]
		public void GenerateTests([ValueSource(nameof(m_Sizes))] uint size, [ValueSource(nameof(m_Heights))] float height)
		{
			World world = new World(size, size);

			// Test whether all chunks' events have been invoked before the world's event is invoked
			bool[,] chunkEventCalled = new bool[world.Chunks.GetLength(0), world.Chunks.GetLength(1)];
			foreach (Chunk chunk in world.Chunks)
			{
				chunk.OnGenerationComplete += (object sender, EventArgs _) => chunkEventCalled[(sender as Chunk).X, (sender as Chunk).Z] = true;
			}
			world.OnGenerationComplete += (object _, EventArgs _) => Assert.That(chunkEventCalled, Has.No.Member(false));

			// NaN represents flat terrain
			world.Generate(new WorldGenerator(float.IsNaN(height), height));

			// Test for correct height map
			foreach (Chunk chunk in world.Chunks)
			{
				// NaN represents flat terrain
				if (float.IsNaN(height))
				{
					Assert.That(chunk.HeightMap, Is.All.EqualTo(height));

					continue;
				}
				
				for (uint x = 0; x < chunk.Size; ++x)
				{
					for (uint z = 0; z < chunk.Size; ++z)
					{
						Assert.AreEqual(chunk.X * chunk.Size + x + chunk.Z * chunk.Size + z, chunk.HeightMap[x, z]);
					}
				}
			}
		}

		[Test]
		public void GenerateTests([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(size, size);

			// Test whether all chunks' events have been invoked before the world's event is invoked
			bool[,] chunkEventCalled = new bool[world.Chunks.GetLength(0), world.Chunks.GetLength(1)];
			foreach (Chunk chunk in world.Chunks)
			{
				chunk.OnTick += (object sender, EventArgs _) => chunkEventCalled[(sender as Chunk).X, (sender as Chunk).Z] = true;
			}
			world.OnTick += (object _, EventArgs _) => Assert.That(chunkEventCalled, Has.No.Member(false));

			world.Tick();
		}
	}
}
