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
using UnityEngine;

using SafariTycoon.Controller;

namespace SafariTycoon.View
{
	[ExecuteAlways]
	[RequireComponent(typeof(WorldController))]
	public class WorldView : MonoBehaviour
	{
		private WorldController m_WorldController;

		public void OnEnable()
		{
			m_WorldController = GetComponent<WorldController>();
		}

		public void Generate(uint worldSize, uint chunkSize)
		{
			m_WorldController.Initialize(worldSize, chunkSize);
			m_WorldController.Generate();
		}

		public void CancelGeneration()
		{
			m_WorldController.CancelGeneration();
		}
	}
}
