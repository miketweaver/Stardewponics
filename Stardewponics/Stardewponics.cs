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
			//var texture = helper.Content.Load<Texture2D>(@"assets\texture.xnb", ContentSource.ModFolder);
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
				this.Monitor.Log("Build Greenhouse key pressed.");
                this.Farm.buildStructure(new Greenhouse(this.Helper.Content.Load<Texture2D>(@"assets\greenhouse.xnb", ContentSource.ModFolder)).SetDaysOfConstructionLeft(0), new Vector2(25, 40), false, Game1.player);

				GameLocation aquaponics = new GameLocation(Game1.content.Load<Map>("..\\Mods\\Stardewponics\\assets\\greenhousemap"), "Aquaponics");
				aquaponics.IsOutdoors = false;
				aquaponics.isFarm = true;
				Game1.locations.Add(aquaponics);
				Game1.locations[1].warps.Add(new Warp(29, 44, "Aquaponics", 10, 22, false));
				Game1.locations[1].warps.Add(new Warp(30, 44, "Aquaponics", 10, 22, false));
			}
			if (e.KeyPressed == Keys.OemOpenBrackets)
			{
				Game1.warpFarmer("Farm", 24, 39, false);
			}
		}

		private void TimeEvents_AfterDayStarted(object sender, EventArgs eventArgs)
		{
				this.Farm = Game1.getFarm();
		}

}
}