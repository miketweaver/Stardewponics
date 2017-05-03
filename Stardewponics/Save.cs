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
        public List<Vector2> GreenHouse = new List<Vector2>();
        public Save() { SaveSeed = ulong.MaxValue; }


        /*********
        ** Public methods
        *********/
        public Save(string nameInput, ulong input)
        {
            SaveSeed = input;
            FarmerName = nameInput;
        }

        public Save AddTractorHouse(int inputX, int inputY)
        {
            foreach (Vector2 THS in GreenHouse)
            {
                if (THS.X == inputX && THS.Y == inputY)
                    return this;
            }
            GreenHouse.Add(new Vector2(inputX, inputY));
            return this;
        }
    }
}
