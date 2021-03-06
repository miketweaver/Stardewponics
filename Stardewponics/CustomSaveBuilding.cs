﻿using Microsoft.Xna.Framework;

namespace Stardewponics
{
	/// <summary>Metadata for a stashed building.</summary>
	internal class CustomSaveBuilding
	{
		/*********
        ** Accessors
        *********/
		/// <summary>The building type.</summary>
		public string Type { get; }

		/// <summary>The tile location.</summary>
		public Vector2 Tile { get; }

        // <summary>Days left in construction</summary>
        public int DaysOfConstructionLeft { get; set; }


		/*********
        ** Public methods
        *********/
		/// <summary>Construct an instance.</summary>
		/// <param name="tile">The building type.</param>
		/// <param name="type">The tile location.</param>
		public CustomSaveBuilding(Vector2 tile, string type, int daysOfConstructionLeft)
		{
			this.Tile = tile;
			this.Type = type;
            this.DaysOfConstructionLeft = daysOfConstructionLeft;
		}
	}
}