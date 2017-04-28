using System;
using Microsoft.Xna.Framework;
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
		SaveEvents.AfterLoad += this.ReceiveAfterLoad;
	}


	/*********
	** Private methods
	*********/
	/// <summary>The method invoked when the player presses a keyboard button.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event data.</param>
	private void ReceiveKeyPress(object sender, EventArgsKeyPressed e)
	{
		this.Monitor.Log($"Player pressed {e.KeyPressed}.");
	}

	/// <summmary>The event handler called after the player loads their save.</summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">The event arguments.</param>
	public void ReceiveAfterLoad(object sender, EventArgs e)
	{
		this.Monitor.Log("The player loaded their game! This is a good time to do things.");
		this.Monitor.Log("Everything in the world is ready to interact with at this point.");

        this.Farm.buildStructure(new Greenhouse().SetDaysOfConstructionLeft(0), new Vector2(5, 35), false, Game1.player);
	}

}
}