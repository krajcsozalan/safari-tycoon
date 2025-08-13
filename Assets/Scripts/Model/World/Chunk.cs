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

namespace SafariTycoon.Model
{
	public class Chunk
	{
		public uint Size { get; private set; }
		public uint X { get; private set; }
		public uint Z { get; private set; }
		public float[,] HeightMap { get; private set; }

		public Chunk(uint size, uint x, uint z)
		{
			Size = size;
			X = x;
			Z = z;
			HeightMap = new float[Size, Size];

			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					HeightMap[i, j] = 0f;
				}
			}
		}

		public void Generate(IWorldGenerator generator)
		{
			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					HeightMap[i, j] = generator.GetHeight(X * Size + i, Z * Size + j);
				}
			}
		}
	}
}
