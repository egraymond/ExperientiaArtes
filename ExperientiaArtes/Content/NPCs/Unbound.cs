using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ExperientiaArtes.Content.NPCs
{
    public class Unbound : ModNPC
    {
        private string originalName = "Unbound";


        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.noGravity = true; // This makes the enemy float in the air
            NPC.noTileCollide = true; // This makes the enemy ignore tiles and collision
            NPC.aiStyle = -1;
        }

        public override void AI()
        {
            // Floating up and down
            NPC.ai[0]++;
            if (NPC.ai[0] >= 120) // 2 seconds cycle (60 frames per second)
            {
                NPC.ai[0] = 0;
            }
            NPC.position.Y += (float)System.Math.Sin(NPC.ai[0] / 60.0f * MathHelper.TwoPi) * 2;
        }
    }
}

