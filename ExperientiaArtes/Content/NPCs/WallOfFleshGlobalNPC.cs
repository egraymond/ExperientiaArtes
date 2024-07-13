using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExperientiaArtes.Content.NPCs
{
    public class WallOfFleshGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.WallofFlesh;
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh && !NPC.downedPlantBoss) // Ensure the item drops only once
            {
                if (!Main.expertMode || !Main.hardMode)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<Items.ArtificialCore>());
                    }
                    NPC.downedPlantBoss = true; // Use this flag to ensure it only drops once
                }
            }
        }
    }
}
