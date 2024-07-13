using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ExperientiaArtes.Content.Items
{
    public class Finis : ModItem
    {

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 20)); // 5 ticks per frame, 20 frames
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Expert;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.consumable = true;
        }

        public override void AddRecipes()
            {
                CreateRecipe()
                .AddIngredient(ModContent.ItemType<ArtificialCore>(), 1)
                .AddIngredient(ModContent.ItemType<SipsaiteOre>(), 65)
                .AddIngredient(ModContent.ItemType<OcularTissue>(), 30)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
            }
        public override bool CanUseItem(Player player)
        {
            return true; // Conditions to use the item can be added here
        }

        public override bool? UseItem(Player player)
        {
            // Play the Lunatic Cultist death sound
            SoundEngine.PlaySound(SoundID.NPCDeath59, player.position);

            // Start the music fade
            Main.NewText("The air feels heavy...", Color.Red);
            StartMusicFade(player);

            return true;
        }

        private void StartMusicFade(Player player)
        {
            player.GetModPlayer<FinisPlayer>().StartCountdown();
        }
    }

    public class FinisPlayer : ModPlayer
    {
        private int countdown = 0;
        private bool fadeComplete = false;

        public override void PostUpdate()
        {
            if (countdown > 0)
            {
                countdown--;
                Player.moveSpeed = 0f; // Slow the player down completely
                Player.controlJump = false; // Disable jumping
                Player.controlUseItem = false; // Disable item use
                Player.controlUseTile = false; // Disable tile use

                if (countdown == 0)
                {
                    TriggerShockwaveAndSpawnNPC();
                    fadeComplete = true;
                }
            }
        }

        public void StartCountdown()
        {
            countdown = 420; // 7 seconds at 60 ticks per second
            fadeComplete = false;
            Main.musicFade[Main.curMusic] = 0f; // Fade out the music
            Main.curMusic = 0; // Stop the music
        }

        private void TriggerShockwaveAndSpawnNPC()
        {
            Player player = Main.LocalPlayer;

            // Create the shockwave effect
            Vector2 position = new Vector2(player.Center.X, player.Center.Y - (20 * 16));
            Projectile.NewProjectile(null, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.Shockwave>(), 0, 0, player.whoAmI);
        }
    }
}


