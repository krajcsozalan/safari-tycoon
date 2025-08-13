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
	public class WorldTests
	{
		private static uint[] m_Sizes = { 0, 1, 2, 3, 5, 7, 8 };

		[Test]
		public void ConstructorTests([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(size, size);

			Assert.That(world.Size, Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(0), Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(1), Is.EqualTo(size));

			Assert.That(world.Chunks, Has.No.Member(null));
		}

		private class WorldGenerator : IWorldGenerator
		{
			public float GetHeight(uint x, uint z)
			{
				return 0f;
			}
		}

		[Test]
		public void GenerateTest([ValueSource(nameof(m_Sizes))] uint size)
		{
			World world = new World(size, size);

			// Pass test if the event is invoked
			world.OnGenerationComplete += (object _, EventArgs _) => Assert.Pass();

			world.Generate(new WorldGenerator());

			// Fail test if the event hasn't been invoked
			Assert.Fail();
		}

		[Test]
		public void TickTest()
		{
			World world = new World(1, 1);

			// Pass test if the tick event is invoked
			world.OnTick += (object _, EventArgs _) => Assert.Pass();

			world.Tick();

			// Fail test if the tick event hasn't been invoked
			Assert.Fail();
		}
	}
}
