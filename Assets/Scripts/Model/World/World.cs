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
using System.Threading.Tasks;

namespace SafariTycoon.Model
{
	/// <summary>
	/// Represents a game world, including <c>Chunk</c>s
	/// </summary>
	public class World : ITickable
	{
		/// <summary>
		/// Side length of the <c>World</c> (in <c>Chunk</c>s)
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// Contains the <c>Chunk</c>s that make up the <c>World</c>
		/// </summary>
		public Chunk[,] Chunks { get; private set; }

		/// <summary>
		/// Invoked after terrain generation has finished
		/// </summary>
		public event EventHandler OnGenerationComplete;

		/// <summary>
		/// Invoked after the <c>World</c> (including <c>Chunk</c>s) has been ticked
		/// </summary>
		public event EventHandler OnTick;

		/// <summary>
		/// Constructs a new world with a valid (however unplayable) state
		/// </summary>
		/// <param name="size">Side length of the <c>World</c> (in <c>Chunk</c>s)</param>
		/// <param name="chunkSize">Side length of <c>Chunk</c>s (in tiles)</param>
		public World(uint size, uint chunkSize)
		{
			Size = size;
			Chunks = new Chunk[Size, Size];

			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					Chunks[i, j] = new Chunk(chunkSize, i, j);
				}
			}
		}

		/// <summary>
		/// Generates the terrain of the <c>World</c>
		/// </summary>
		/// <param name="generator">Generator used to calculate the terrain height map</param>
		public void Generate(IWorldGenerator generator)
		{
			Task[] tasks = new Task[Size * Size];

			for (uint i = 0; i < Size; ++i)
			{
				for (uint j = 0; j < Size; ++j)
				{
					Chunk chunk = Chunks[i, j];
					tasks[i * Size + j] = Task.Run(() => chunk.Generate(generator));
				}
			}

			Task.WaitAll(tasks);

			OnGenerationComplete?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Ticks the <c>World</c>, including <c>Chunk</c>s
		/// </summary>
		public void Tick()
		{
			foreach (Chunk chunk in Chunks)
			{
				chunk.Tick();
			}

			OnTick?.Invoke(this, EventArgs.Empty);
		}
	}
}
