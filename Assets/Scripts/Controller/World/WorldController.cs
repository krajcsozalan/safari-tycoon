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
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

using SafariTycoon.Model;

namespace SafariTycoon.Controller
{
	public class WorldController : MonoBehaviour
	{
		public CancellationTokenSource CancellationTokenSource { get; private set; }

		public World World { get; private set; }

		public void Initialize(uint worldSize, uint chunkSize)
		{
			World = new World(worldSize, chunkSize);

			CancellationTokenSource = new CancellationTokenSource();
		}

		public void Generate()
		{
			Task.Run(() => World.GenerateAsync(new WorldGenerator(), CancellationTokenSource.Token));
		}

		public void CancelGeneration()
		{
			CancellationTokenSource?.Cancel();
		}

		private class WorldGenerator : IWorldGenerator
		{
			public float GetHeight(uint x, uint z)
			{
				return 0f;
			}
		};
	}
}
