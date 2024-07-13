using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExperientiaArtes.Content.Items
{
    public class OcularTissue : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Green;
            Item.material = true; // Indicates this item is a material
        }
    }
}
