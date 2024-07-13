using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExperientiaArtes.Content.Items
{
    public class ArtificialCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 17)); // 5 ticks per frame, 20 frames in total
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Expert; // Adjust rarity as needed
            Item.material = true; // Indicates this item is a material for crafting
        }
    }
}
