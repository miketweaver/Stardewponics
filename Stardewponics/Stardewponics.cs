using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

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
				this.Monitor.Log("Build Greenhouse key pressed.");
				this.Farm.buildStructure(new Greenhouse().SetDaysOfConstructionLeft(0), new Vector2(25, 40), false, Game1.player);
		}
	}

	private void TimeEvents_AfterDayStarted(object sender, EventArgs eventArgs)
	{
			this.Farm = Game1.getFarm();
	}

}
}