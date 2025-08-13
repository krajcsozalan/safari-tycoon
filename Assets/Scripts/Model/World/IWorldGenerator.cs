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
	/// <summary>
	/// Generator used to calculate the height map for a <c>World</c> terrain
	/// </summary>
	public interface IWorldGenerator
	{
		/// <summary>
		/// Calculates the height of the terrain at a given tile
		/// </summary>
		/// <param name="x">X coordinate of the tile (absolute)</param>
		/// <param name="z">Z coordinate of the tile (absolute)</param>
		/// <returns>Y coordinate of the tile (with 0 being sea level)</returns>
		public float GetHeight(uint x, uint z);
	}
}
