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
using System.Collections;

using NUnit.Framework;

using UnityEngine;
using UnityEngine.TestTools;

using SafariTycoon.Model;

namespace SafariTycoon.Tests
{
	public class ChunkTests
	{
		private static uint[] m_Sizes = { 0, 1, 2, 3, 4 };
		private static float[] m_Heights = { -1f, 0f, 1f, 1.5f };

		[Test]
		public void ConstructorTest([ValueSource(nameof(m_Sizes))] uint size)
		{
			uint x = size, z = size;

			Chunk chunk = new Chunk(x, z, size);

			Assert.That(chunk.X, Is.EqualTo(x));
			Assert.That(chunk.Z, Is.EqualTo(z));
			Assert.That(chunk.Size, Is.EqualTo(size));
			Assert.That(chunk.HeightMap.GetLength(0), Is.EqualTo(size));
			Assert.That(chunk.HeightMap.GetLength(1), Is.EqualTo(size));
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
			Chunk chunk = new Chunk(1, 1, 16);
			chunk.Generate(new ConstantWorldGenerator(height));

			foreach (float y in chunk.HeightMap)
			{
				Assert.That(y, Is.EqualTo(height));
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
			Chunk chunk = new Chunk(1, 1, 16);
			chunk.Generate(new LinearWorldGenerator());
			
			for (uint i = 0; i < chunk.Size; ++i)
			{
				for (uint j = 0; j < chunk.Size; ++j)
				{
					Assert.That(chunk.HeightMap[i, j], Is.EqualTo(chunk.X * chunk.Size + i + chunk.Z * chunk.Size + j));
				}
			}
		}
	}
}
