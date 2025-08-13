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

namespace SafariTycoon.Model
{
	/// <summary>
	/// Represents a chunk of a <c>World</c>
	/// </summary>
	public class Chunk : ITickable
	{
		/// <summary>
		/// Side length of the <c>Chunk</c> (in tiles)
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// X coordinate of the <c>Chunk</c> in the <c>World</c> (absolute)
		/// </summary>
		public uint X { get; private set; }

		/// <summary>
		/// Z coordinate of the <c>Chunk</c> in the <c>World</c> (absolute)
		/// </summary>
		public uint Z { get; private set; }

		/// <summary>
		/// Contains the heights of the tiles that make up the <c>Chunk</c>
		/// </summary>
		public float[,] HeightMap { get; private set; }

		/// <summary>
		/// Invoked after terrain generation has finished
		/// </summary>
		public event EventHandler OnGenerationComplete;

		/// <summary>
		/// Invoked after the <c>Chunk</c> has been ticked
		/// </summary>
		public event EventHandler OnTick;

		/// <summary>
		/// Constructs a new chunk with a valid (however unplayable) state
		/// </summary>
		/// <param name="size">Side length of the <c>Chunk</c> (in tiles)</param>
		/// <param name="x">X coordinate of the <c>Chunk</c> in the <c>World</c> (absolute)</param>
		/// <param name="z">Z coordinate of the <c>Chunk</c> in the <c>World</c> (absolute)</param>
		public Chunk(uint size, uint x, uint z)
		{
			Size = size;
			X = x;
			Z = z;
			HeightMap = new float[Size, Size];
		}

		/// <summary>
		/// Generates the terrain of the <c>Chunk</c>
		/// </summary>
		/// <param name="generator">Generator used to calculate the terrain height map</param>
		public void Generate(IWorldGenerator generator)
		{
			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					HeightMap[i, j] = generator.GetHeight(X * Size + i, Z * Size + j);
				}
			}

			OnGenerationComplete?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Ticks the <c>Chunk</c>
		/// </summary>
		public void Tick()
		{
			OnTick?.Invoke(this, EventArgs.Empty);
		}
	}
}
