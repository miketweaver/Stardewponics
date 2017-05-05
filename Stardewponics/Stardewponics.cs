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
using System.Collections.Generic;

namespace Stardewponics
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		/*********
		** Properties
		*********/
		private Farm Farm = null;
		private bool IsNewDay;
		private SaveCollection AllSaves;

		/*********
		** Public methods
		*********/
		/// <summary>Initialise the mod.</summary>
		/// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
		public override void Entry(IModHelper helper)
		{
			ControlEvents.KeyPressed += this.ReceiveKeyPress;
			ControlEvents.KeyPressed += this.TimeEvents_AfterDayStarted;
			MenuEvents.MenuChanged += this.MenuAddInBuilding;

			// spawn tractor & remove it before save
			TimeEvents.AfterDayStarted += this.TimeEvents_AfterDayStarted;
			LocationEvents.CurrentLocationChanged += this.LocationEvents_CurrentLocationChanged;
			SaveEvents.BeforeSave += this.SaveEvents_BeforeSave;

		}


		/*********
		** Private methods
		*********/
		/// <summary>The method invoked when the player presses a keyboard button.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>

		private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
		{
			// spawn tractor house & tractor
			if (this.IsNewDay && e.NewLocation == this.Farm)
			{
				this.LoadModInfo();
				this.IsNewDay = false;
			}
		}

		private void SaveEvents_BeforeSave(object sender, EventArgs eventArgs)
		{
			// save mod data
			this.SaveModInfo();

			// remove tractor from save
			foreach (Building aquabuilding in this.Farm.buildings.ToArray())
				if (aquabuilding.buildingType == "Aquaponics")
					this.Farm.destroyStructure(aquabuilding);

			//Not needed as we'll be anly placing in the Farm?
			//foreach (GameLocation location in Game1.locations)
				
			//	this.RemoveEveryCharactersOfType<Tractor>(location);
		}


		//use to write AllSaves info to some .json file to store save
		private void SaveModInfo()
		{
			if (AllSaves == null)
				AllSaves = new SaveCollection().Add(new Save(Game1.player.name, Game1.uniqueIDForThisGame));

			Save currentSave = AllSaves.FindSave(Game1.player.name, Game1.uniqueIDForThisGame);

			if (currentSave.SaveSeed != ulong.MaxValue)
			{
				currentSave.GreenHouse.Clear();
				foreach (Building b in this.Farm.buildings)
				{
					if (b is Building && b.buildingType == "Aquaponics")
						currentSave.AddTractorHouse(b.tileX, b.tileY);
				}
			}
			else
			{
				AllSaves.saves.Add(new Save(Game1.player.name, Game1.uniqueIDForThisGame));
				SaveModInfo();
				return;
			}
			this.Helper.WriteJsonFile("AquaponicsSave.json", AllSaves);
		}

		//use to load save info from some .json file to AllSaves
		private void LoadModInfo()
		{
			this.AllSaves = this.Helper.ReadJsonFile<SaveCollection>("AquaponicsSave.json") ?? new SaveCollection();
			Save saveInfo = this.AllSaves.FindSave(Game1.player.name, Game1.uniqueIDForThisGame);
			if (saveInfo != null && saveInfo.SaveSeed != ulong.MaxValue)
			{
				foreach (Vector2 THS in saveInfo.GreenHouse)
				{
					Building loadGreen = new Building(CreateGreenhouse(), THS);
					loadGreen.daysOfConstructionLeft = 0;
					this.Farm.buildStructure(loadGreen, THS, false, Game1.player);
				}
			} 
		}



		private void ReceiveKeyPress(object sender, EventArgsKeyPressed e)
		{
			if (e.KeyPressed == Keys.OemCloseBrackets)
			{

				int start = 30;
				int farmX = 25;
				int farmY = 40;
				this.Monitor.Log("Build Greenhouse key pressed. ]");
				this.Farm.buildStructure(new Greenhouse(this.Helper.Content).SetDaysOfConstructionLeft(0), new Vector2(farmX, farmY), false, Game1.player);



				GameLocation farmLocation = Game1.getLocationFromName("Farm");

				farmLocation.warps.Add(new Warp(farmX + 4, farmY + 4, "Greenhouse", start + 10, start + 23, false));
				farmLocation.warps.Add(new Warp(farmX + 5, farmY + 4, "Greenhouse", start + 10, start + 23, false));

				Game1.locations[1].Name

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
							var aquaTile = aqualayer.Tiles[x, y];
							if (aquaTile != null)
								layer.Tiles[start + x, start + y] = new StaticTile(layer, tilesheet, BlendMode.Alpha, aquaTile.TileIndex);
						}
					}
				}

				greenhouseLocation.warps.Add(new Warp(start + 10, start + 24, "Farm", farmX + 4, farmY + 7, false));
			}
			if (e.KeyPressed == Keys.OemOpenBrackets)
			{
                this.Monitor.Log("Warp key pressed. [");
				//Game1.warpFarmer("Greenhouse", 15, 22, false);
				//Game1.warpFarmer("Farm", 29, 47, false);
				Game1.warpFarmer("Mountain", 12, 27, false);
			}

			if (e.KeyPressed == Keys.L)
			{
                this.Monitor.Log("L key pressed.");
				Building currentBuilt = this.Helper.Reflection.GetPrivateValue<Building>(Game1.activeClickableMenu, "currentBuilding");
				this.Monitor.Log(currentBuilt.buildingType.ToString());
				this.Monitor.Log(currentBuilt.GetType().FullName);
			}
		}

		private void TimeEvents_AfterDayStarted(object sender, EventArgs eventArgs)
		{
				this.Farm = Game1.getFarm();
		}

		private void MenuAddInBuilding(object sender, EventArgsClickableMenuChanged e)
		{

			if (e.NewMenu is CarpenterMenu)
			{


				List<BluePrint> blueprints = this.Helper.Reflection.GetPrivateValue<List<BluePrint>>(Game1.activeClickableMenu, "blueprints");

				//Building currentBuilt = this.Helper.Reflection.GetPrivateValue<Building>(Game1.activeClickableMenu, "currentBuilding");
				//currentBuilt = 
				foreach (BluePrint print in blueprints)
				{
					this.Monitor.Log(print.name);
					//this.Monitor.Log(print.texture.Width.ToString());
					this.Monitor.Log(print.sourceRectForMenuView.Width.ToString());
                    this.Monitor.Log(print.sourceRectForMenuView.Height.ToString());
				}



				blueprints.Add(CreateGreenhouse());
			}
			else
			{
				this.Monitor.Log(e.NewMenu.ToString());
			}

		}

		private BluePrint CreateGreenhouse()
		{

				BluePrint AquaBP = new BluePrint("Aquaponics");
				AquaBP.itemsRequired.Clear();

				string[] strArray2 = "390 200".Split(' ');
				int index = 0;
				while (index<strArray2.Length)
				{
					if (!strArray2[index].Equals(""))
						AquaBP.itemsRequired.Add(Convert.ToInt32(strArray2[index]), Convert.ToInt32(strArray2[index + 1]));
					index += 2;
				}
				AquaBP.texture = this.Helper.Content.Load<Texture2D>(@"assets\greenhouse.xnb", ContentSource.ModFolder);
				AquaBP.humanDoor = new Point(-1, -1);
				AquaBP.animalDoor = new Point(-2, -1);
				AquaBP.mapToWarpTo = "null";
				AquaBP.displayName = "Aquaponics";
				AquaBP.description = "A place to grow plants using fertilized water from your Fish!";
				AquaBP.blueprintType = "Buildings";
				AquaBP.nameOfBuildingToUpgrade = "";
				AquaBP.actionBehavior = "null";
				AquaBP.maxOccupants = -1;
				AquaBP.moneyRequired = 100; //ModConfig.TractorHousePrice;
				AquaBP.tilesWidth = 14;
				AquaBP.tilesHeight = 7;
				AquaBP.getTileSheetIndexForStructurePlacementTile(0, 0);
				AquaBP.sourceRectForMenuView = new Microsoft.Xna.Framework.Rectangle(0, 0, 96, 96);
				AquaBP.namesOfOkayBuildingLocations.Clear();
				AquaBP.namesOfOkayBuildingLocations.Add("Farm");
				AquaBP.magical = false;
			return AquaBP;
		}
}
}