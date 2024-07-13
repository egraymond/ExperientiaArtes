using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExperientiaArtes.Content.Tiles
{
    public class SipsaiteOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 900;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 350;

            AddMapEntry(new Color(11, 74, 8), CreateMapEntryName());

            DustType = DustID.GreenTorch;
            HitSound = SoundID.Tink;

            MineResist = 1.5f;
            MinPick = 60;
        }
        public override bool CanExplode(int i, int j)
        {
            return false; // Cannot be destroyed by explosives
        }
    }
}