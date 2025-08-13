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
			World world = new World(size, 1);

			Assert.That(world.Size, Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(0), Is.EqualTo(size));
			Assert.That(world.Chunks.GetLength(1), Is.EqualTo(size));

			Assert.That(world.Chunks, Has.No.Member(null));
		}
	}
}
