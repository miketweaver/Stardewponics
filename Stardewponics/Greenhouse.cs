using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;

namespace Stardewponics
{
	public class Greenhouse : Building
	{
		/*********
		** Public methods
		*********/
		public Greenhouse(Texture2D texture) : base()
		{
			buildingType = "Fancy Greenhouse";
			humanDoor = new Point(-1, -1);
			animalDoor = new Point(-2, -1);
			indoors = null;
			nameOfIndoors = "";
			baseNameOfIndoors = "";
			nameOfIndoorsWithoutUnique = "";
			magical = false;
			tileX = 0;
			tileY = 0;
			maxOccupants = 0;
			tilesWide = 15;
			tilesHigh = 7;
			this.texture = texture;
			//texture = Game1.content.Load<Texture2D>("..\\Mods\\Stardewponics\\assets\\greenhouse");
			daysOfConstructionLeft = 1;
			leftShadow = new Rectangle(656, 394, 16, 16); //656, 394, 16, 16
		}

		public Greenhouse SetDaysOfConstructionLeft(int input)
		{
			daysOfConstructionLeft = input;
			return this;
		}

		public override bool intersects(Rectangle boundingBox)
		{
			if (!base.intersects(boundingBox))
				return false;
			if (boundingBox.X >= (this.tileX + 4) * Game1.tileSize && boundingBox.Right < (this.tileX + 7) * Game1.tileSize)
				return boundingBox.Y <= (this.tileY + 1) * Game1.tileSize;
			return true;
		}


		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				this.drawInConstruction(b);
			}
			else
			{
				this.drawShadow(b, -1, -1);
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 2) * Game1.tileSize) / 10000f);
			}
		}

		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft > 0)
				return;
			//this.grabHorse();
			//do special action for this building here
			/*
			 */
		}

		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height);   
		}
	}
}