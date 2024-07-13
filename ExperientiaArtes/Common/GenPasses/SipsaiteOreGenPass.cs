using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.WorldBuilding;
using ExperientiaArtes.Content.Tiles;
using Terraria.ID;

namespace ExperientiaArtes.Common.Systems.GenPasses
{
    internal class SipsaiteOreGenPass : GenPass
    {
        public SipsaiteOreGenPass(string name, float weight) : base(name, weight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spawning Sipsaite Ores";

            // Sipsaite Ore
            int maxToSpawn = (int)(Main.maxTilesX * Main.maxTilesY * 0.0001);
            for (int i = 0; i < maxToSpawn; i++)
            {
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY - 300);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 21), WorldGen.genRand.Next(6, 9), ModContent.TileType<SipsaiteOre>());
            }
        }
    }
}