using System.Collections.Generic;
using Microsoft.Xna.Framework;

//TractorMod

namespace Stardewponics
{
    public class Save
    {
        /*********
        ** Accessors
        *********/
        public string FarmerName { get; set; } = "";
        public ulong SaveSeed { get; set; }
        public List<Vector2> GreenHouseList = new List<Vector2>();
        public Save() { SaveSeed = ulong.MaxValue; }
        public int daysOfConstructionLeft { get; set; }


        /*********
        ** Public methods
        *********/
        public Save(string nameInput, ulong input)
        {
            SaveSeed = input;
            FarmerName = nameInput;
        }

        public Save AddCustomBuilding(int inputX, int inputY)
        {
            foreach (Vector2 THS in GreenHouseList)
            {
                if (THS.X == inputX && THS.Y == inputY)
                {
                    return this;
                }
            }
            GreenHouseList.Add(new Vector2(inputX, inputY));
            return this;
        }
    }
}
