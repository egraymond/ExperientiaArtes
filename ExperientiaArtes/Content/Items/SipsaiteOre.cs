using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExperientiaArtes.Content.Items
{
    public class SipsaiteOre : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(copper: 60);
            Item.rare = ItemRarityID.Orange; // Orange rarity
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.SipsaiteOre>();
            Item.material = true;
        }
    }
}
