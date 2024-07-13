using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace ExperientiaArtes.Content.Projectiles
{
    public class Shockwave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9; // Set the number of frames for the animation
        }

        public override void SetDefaults()
        {
            Projectile.width = 100; // Adjust width as per your texture size
            Projectile.height = 100; // Adjust height as per your texture size
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 45; // Each frame lasts 5 ticks (9 frames * 5 ticks = 45 ticks)
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Play the animation
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = Main.projFrames[Projectile.type] - 1; // Stop at the last frame
                    // Display message and force quit the game
                    DisplayMessageAndExit();
                }
            }

            // Spawn the Unbound NPC halfway through the animation
            if (Projectile.frame == 4 && Projectile.frameCounter == 0)
            {
                int npcIndex = NPC.NewNPC(null, (int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), ModContent.NPCType<NPCs.Unbound>());
                if (npcIndex >= 0 && npcIndex < 200)
                {
                    Main.npc[npcIndex].velocity = Vector2.Zero;
                }
            }
        }

        private void DisplayMessageAndExit()
        {
            MessageBox.Show("All Worlds Must End", "End", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Draw the projectile animation
            Texture2D texture = ModContent.Request<Texture2D>("ExperientiaArtes/Content/Projectiles/Shockwave").Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRectangle = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
