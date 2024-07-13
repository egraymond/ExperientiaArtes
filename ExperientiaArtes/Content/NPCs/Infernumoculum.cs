using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace ExperientiaArtes.Content.NPCs
{
    public class Infernumoculum : ModNPC
    {
        private bool spottedPlayer = false;
        private int spottedCooldown = 0;
        private bool fleeing = false;
        private float spottedRange = 480f; // Initial spotting range

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 17; // Number of frames in the sprite sheet
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 34;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 600;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.noGravity = true; // This makes the enemy float in the air
            NPC.noTileCollide = true; // This makes the enemy ignore tiles and collision
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Infernumoculum>()) ? 0.7f : 0f; // Spawns at night on the overworld if no other instances exist
        }

        public override void AI()
        {
            if (spottedCooldown > 0)
            {
                spottedCooldown--;
            }

            if (Main.dayTime && !fleeing)
            {
                fleeing = true;
                NPC.netUpdate = true; // Sync the change with other clients in multiplayer
            }

            if (fleeing)
            {
                FleeBehavior();
            }
            else if (!spottedPlayer)
            {
                IdleBehavior();
            }
            else
            {
                SpottedBehavior();
            }
        }

        private void IdleBehavior()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            //Float up and down
            NPC.ai[0]++;
            if (NPC.ai[0] >= 600) // 10 seconds cycle (60 frames per second)
            {
                NPC.ai[0] = 0;
            }
            NPC.position.Y += (float)Math.Sin(NPC.ai[0] / 100.0f * MathHelper.TwoPi) * 2;

            //Move in a random direction
            if (NPC.ai[1] == 0)
            {
                NPC.ai[1] = Main.rand.NextBool() ? 1 : -1;
            }
            NPC.velocity.X = NPC.ai[1];

            //Check if player is within 30 blocks (480 pixels)
            if (player.Distance(NPC.Center) < spottedRange)
            {
                spottedPlayer = true;
                spottedRange = 960f; // Increase spotting range by an additional 30 blocks (480 pixels)
                NPC.netUpdate = true; // Sync the change with other clients in multiplayer
            }

            //Ensure the enemy spawns in the sky
            if (NPC.position.Y > player.position.Y - 160f) // Ensure it is at least 20 blocks above the player
            {
                NPC.position.Y = player.position.Y - 160f;
            }
        }

        private void SpottedBehavior()
        {
            Player player = Main.player[NPC.target];

            //Target position is 10 blocks above the player
            Vector2 targetPosition = new Vector2(player.position.X, player.position.Y - 160f); // 10 blocks above the player

            //Calculate the direction vector to the target position
            Vector2 direction = targetPosition - NPC.Center;
            direction.Normalize();

            //Slowed down acceleration towards the target
            float acceleration = 0.03f; // Adjust the acceleration as needed (slightly slower)
            NPC.velocity += direction * acceleration;

            //Cap the maximum speed
            float maxSpeed = 6f; // Adjust the maximum speed as needed (slightly slower)
            if (NPC.velocity.Length() > maxSpeed)
            {
                NPC.velocity = Vector2.Normalize(NPC.velocity) * maxSpeed;
            }

            //Hover smoothly above the player when within 5 blocks of the player's X position
            if (Math.Abs(player.position.X - NPC.position.X) <= 16f) // 5 blocks (16 pixels per block)
            {
                NPC.velocity.X = 0;
                NPC.position.X = player.position.X;
            }

            //Smooth vertical movement to stay 10 blocks above the player
            float verticalSpeed = 0.1f; // Adjust vertical speed as needed
            if (NPC.position.Y > player.position.Y - 160f)
            {
                NPC.velocity.Y = -verticalSpeed;
            }
            else if (NPC.position.Y < player.position.Y - 160f)
            {
                NPC.velocity.Y = verticalSpeed;
            }

            //Apply the "Broken Armor" debuff to the player
            player.AddBuff(BuffID.BrokenArmor, 2);

            //Return to Idle if player is out of range
            if (player.Distance(NPC.Center) > spottedRange)
            {
                spottedPlayer = false;
                spottedRange = 480f; // Reset spotting range
                spottedCooldown = 60; // 1 second cooldown to prevent immediate re-spotting
                NPC.netUpdate = true; // Sync the change with other clients in multiplayer
            }
        }

        private void FleeBehavior()
        {
            Player player = Main.player[NPC.target];
            
            //Flee in a curved direction away from the player
            Vector2 fleeDirection = new Vector2(1f, -0.5f); // Adjust direction as needed
            fleeDirection.Normalize();

            //Slowed down acceleration away from the player
            float acceleration = 0.03f; // Adjust the acceleration as needed (slightly slower)
            NPC.velocity += fleeDirection * acceleration;

            //Cap the maximum speed
            float maxSpeed = 8f; // Adjust the maximum speed as needed (slightly slower)
            if (NPC.velocity.Length() > maxSpeed)
            {
                NPC.velocity = Vector2.Normalize(NPC.velocity) * maxSpeed;
            }

            //Despawn the enemy when it's far enough
            if (NPC.Distance(player.Center) > 1000f)
            {
                NPC.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.2;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnKill()
        {
            //Spawn demon eye dust particles upon death
            for (int i = 0; i < 1000; i++)
            {
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimtaneWeapons);
                Main.dust[dustIndex].velocity *= 1.5f;
            }

            //Drop randomized number of bronze and silver coins
            int bronzeCoins = Main.rand.Next(1, 100); // Between 1 and 99
            int silverCoins = Main.rand.Next(4, 7); // Between 4 and 6
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.CopperCoin, bronzeCoins);
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.SilverCoin, silverCoins);

            //Drop Ocular Tissue item
            int ocularTissue = Main.rand.Next(0, 3); // Between 0 and 2
            if (ocularTissue > 0)
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<Items.OcularTissue>(), ocularTissue);
            }
        }
    }
}