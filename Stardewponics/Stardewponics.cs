using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace Stardewponics
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
{
	/*********
	** Properties
	*********/
	private Farm Farm = null;
	/*********
	** Public methods
	*********/
	/// <summary>Initialise the mod.</summary>
	/// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
	public override void Entry(IModHelper helper)
	{
			ControlEvents.KeyPressed += this.ReceiveKeyPress;
			ControlEvents.KeyPressed += this.TimeEvents_AfterDayStarted;
	}


		/*********
		** Private methods
		*********/
		/// <summary>The method invoked when the player presses a keyboard button.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void ReceiveKeyPress(object sender, EventArgsKeyPressed e)
		{
			if (e.KeyPressed == Keys.OemCloseBrackets)
			{

				int start = 30;
				int farmX = 25;
				int farmY = 40;
				this.Monitor.Log("Build Greenhouse key pressed.");
				this.Farm.buildStructure(new Greenhouse(this.Helper.Content).SetDaysOfConstructionLeft(0), new Vector2(farmX, farmY), false, Game1.player);



				GameLocation farmLocation = Game1.getLocationFromName("Farm");

				farmLocation.warps.Add(new Warp(farmX + 4, farmY + 4, "Greenhouse", start + 10, start + 23, false));
				farmLocation.warps.Add(new Warp(farmX + 5, farmY + 4, "Greenhouse", start + 10, start + 23, false));

				GameLocation greenhouseLocation = Game1.getLocationFromName("Greenhouse");
				var tilesheet = greenhouseLocation.map.GetTileSheet("untitled tile sheet");


				var aquaponics = this.Helper.Content.Load<Map>(@"assets\greenhousemap.xnb", ContentSource.ModFolder);
				var layers = new[] { "Back", "Buildings", "Front" };
				foreach (string lay in layers)
				{
				var aqualayer = aquaponics.GetLayer(lay);
				var layer = greenhouseLocation.map.GetLayer(lay);
				layer.LayerSize = new xTile.Dimensions.Size(230, 230);

					for (int x = 0; x<aqualayer.LayerSize.Width; x++)
					{
						for (int y = 0; y<aqualayer.LayerSize.Height; y++)
						{
							try
							{
								layer.Tiles[start + x, start + y] = new StaticTile(layer, tilesheet, BlendMode.Alpha, aqualayer.Tiles[x, y].TileIndex);
							}
							// It will fail if .TileIndex returns null, so we need to handle that.
							catch (Exception ex)
							{
							}
						}

					}
				}

				greenhouseLocation.warps.Add(new Warp(start + 10, start + 24, "Farm", farmX + 4, farmY + 7, false));
			}
			if (e.KeyPressed == Keys.OemOpenBrackets)
			{
				//Game1.warpFarmer("Greenhouse", 15, 22, false);
				Game1.warpFarmer("Farm", 29, 47, false);
			}

			if (e.KeyPressed == Keys.L)
			{


			}
		}

		private void TimeEvents_AfterDayStarted(object sender, EventArgs eventArgs)
		{
				this.Farm = Game1.getFarm();
		}

}
}