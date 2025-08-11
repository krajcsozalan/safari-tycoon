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
namespace SafariTycoon.Model
{
	public class World
	{
		public uint Size { get; private set; }
		public Chunk[,] Chunks { get; private set; }

		#nullable enable
		public delegate void WorldGenerationCompleteEventHandler(object? sender);
		public event WorldGenerationCompleteEventHandler GenerationComplete;
		#nullable disable

		public World(uint size)
		{
			Size = size;
			Chunks = new Chunk[Size, Size];
		}

		public void Generate(IWorldGenerator generator)
		{
			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					Chunks[i, j].Generate(generator);
				}
			}

			GenerationComplete?.Invoke(this);
		}
	}
}
