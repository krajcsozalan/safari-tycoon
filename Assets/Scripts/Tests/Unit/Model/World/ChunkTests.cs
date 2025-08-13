using System;

using NUnit.Framework;

using SafariTycoon.Model;

namespace SafariTycoon.Tests
{
	public class ChunkTests
	{
		private static uint[] m_Sizes = { 0, 1, 2, 3, 5, 7, 8 };

		[Test]
		public void ConstructorTests([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(1, size);
			Chunk chunk = world.Chunks[0, 0];

			Assert.That(chunk.Size, Is.EqualTo(size));
			Assert.That(chunk.HeightMap.GetLength(0), Is.EqualTo(size));
			Assert.That(chunk.HeightMap.GetLength(1), Is.EqualTo(size));
		}

		private class ConstantWorldGenerator : IWorldGenerator
		{
			public float GetHeight(uint x, uint z)
			{
				return (float)Math.E;
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
		public void GenerateTests([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(size, size);
			world.Generate(new ConstantWorldGenerator());

			for (uint i = 0; i < world.Size; ++i)
			{
				for (uint j = 0; j < world.Size; ++j)
				{
					Chunk chunk = world.Chunks[i, j];

					for (uint x = 0; x < chunk.Size; ++x)
					{
						for (uint z = 0; z < chunk.Size; ++z)
						{
							Assert.AreEqual((float)Math.E, chunk.HeightMap[x, z]);
						}
					}
				}
			}

			world.Generate(new LinearWorldGenerator());

			for (uint i = 0; i < world.Size; ++i)
			{
				for (uint j = 0; j < world.Size; ++j)
				{
					Chunk chunk = world.Chunks[i, j];

					for (uint x = 0; x < chunk.Size; ++x)
					{
						for (uint z = 0; z < chunk.Size; ++z)
						{
							Assert.AreEqual(i * chunk.Size + x + j * chunk.Size + z, chunk.HeightMap[x, z]);
						}
					}
				}
			}
		}
	}
}
