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
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

using SafariTycoon.Model;

namespace SafariTycoon.Tests
{
	public class WorldTests
	{
		private static uint[] m_Sizes = { 0, 1, 2, 3, 4 };
		private static float[] m_Heights = { -1f, 0f, 1f, 1.5f };
		private static int[] m_Delays = { 5, 25, 50, 100 };

		[Test]
		public void ConstructorTest([ValueSource(nameof(m_Sizes))] uint size)
		{
			uint chunkSize = size;

			World world = new World(size, chunkSize);

			Assert.That(world.Size, Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(0), Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(1), Is.EqualTo(size));

			Assert.That(world.Chunks, Has.None.Null);
			for (uint i = 0; i < world.Size; ++i)
			{
				for (uint j = 0; j < world.Size; ++j)
				{
					Assert.That(world.Chunks[i, j].X, Is.EqualTo(i));
					Assert.That(world.Chunks[i, j].Z, Is.EqualTo(j));
					Assert.That(world.Chunks[i, j].Size, Is.EqualTo(chunkSize));
				}
			}
		}

		private class ConstantWorldGenerator : IWorldGenerator
		{
			private float m_Height;

			public ConstantWorldGenerator(float height)
			{
				m_Height = height;
			}

			public float GetHeight(uint x, uint z)
			{
				return m_Height;
			}
		}

		[Test]
		public void GenerateTest_Constant([ValueSource(nameof(m_Heights))] float height)
		{
			World world = new World(16, 16);
			world.Generate(new ConstantWorldGenerator(height));

			foreach (Chunk chunk in world.Chunks)
			{
				foreach (float y in chunk.HeightMap)
				{
					Assert.That(y, Is.EqualTo(height));
				}
			}
		}

		[Test]
		public void GenerateAsyncTest_Constant([ValueSource(nameof(m_Heights))] float height)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

			World world = new World(16, 16);
			world.GenerateAsync(new ConstantWorldGenerator(height), cancellationTokenSource.Token);

			foreach (Chunk chunk in world.Chunks)
			{
				foreach (float y in chunk.HeightMap)
				{
					Assert.That(y, Is.EqualTo(height));
				}
			}
		}

		private class LinearWorldGenerator : IWorldGenerator
		{
			public float GetHeight(uint x, uint z)
			{
				return x + z;
			}
		}

		[Test]
		public void GenerateTest_Linear()
		{
			World world = new World(16, 16);
			world.Generate(new LinearWorldGenerator());

			foreach (Chunk chunk in world.Chunks)
			{
				for (uint i = 0; i < chunk.Size; ++i)
				{
					for (uint j = 0; j < chunk.Size; ++j)
					{
						Assert.That(chunk.HeightMap[i, j], Is.EqualTo(chunk.X * chunk.Size + i + chunk.Z * chunk.Size + j));
					}
				}
			}
		}

		[Test]
		public void GenerateAsyncTest_Linear()
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

			World world = new World(16, 16);
			world.GenerateAsync(new LinearWorldGenerator(), cancellationTokenSource.Token);

			foreach (Chunk chunk in world.Chunks)
			{
				for (uint i = 0; i < chunk.Size; ++i)
				{
					for (uint j = 0; j < chunk.Size; ++j)
					{
						Assert.That(chunk.HeightMap[i, j], Is.EqualTo(chunk.X * chunk.Size + i + chunk.Z * chunk.Size + j));
					}
				}
			}
		}

		[Test]
		public void GenerateAsyncTest([ValueSource(nameof(m_Delays))] int delay)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

			World world = new World(16, 16);

			Task.Run(async () =>
			{
				await Task.Delay(delay);
				cancellationTokenSource.Cancel();
			});

			world.GenerateAsync(new LinearWorldGenerator(), cancellationTokenSource.Token);
		}
	}
}
