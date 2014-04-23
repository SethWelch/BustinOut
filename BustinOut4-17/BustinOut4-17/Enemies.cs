using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BustinOutMegaMan
{
    class Enemies
    {
        public List<Enemy> enemies1 = new List<Enemy>();
        public List<Enemy> enemies2 = new List<Enemy>();
        public List<Enemy> enemies3 = new List<Enemy>();

        private Texture2D bullet;

        public void LoadContent(ContentManager Content)
        {
            //instantiate the enemies for level 1 on the 
            //3 different screens and load their content
            enemies1.Add(new Enemy(2, "Enemy1B", 1550, 705, 0, 3));
            enemies1.Add(new Enemy(2, "Enemy1B", 850, 705, 0, 3));
            enemies1.Add(new Enemy(1, "Enemy1A", 1350, 285, 1000, 2));
            enemies1.Add(new Enemy(1, "Enemy1A", 1050, 285, 1000, 3));
            enemies1.Add(new Enemy(1, "Enemy1A", 750, 285, 850, 2));
            enemies1.Add(new Enemy(1, "Enemy1A", 450, 285, 1350, 1));

            enemies2.Add(new Enemy(1, "Enemy1A", 450, 285, 850, 2));
            enemies2.Add(new Enemy(1, "Enemy1A", 1240, 285, 1300, 2));
            enemies2.Add(new Enemy(2, "Enemy1B", 1550, 685, 0, 3));
            enemies2.Add(new Enemy(1, "Enemy1A", 350, 715, 900, 2));
            enemies2.Add(new Enemy(2, "Enemy1B", 800, 285, 0, 3));
            enemies2.Add(new Enemy(1, "Enemy1A", 1130, 720, 1500, 1));

            enemies3.Add(new Enemy(2, "Enemy1B", 1590, 265, 0, 3));
            enemies3.Add(new Enemy(1, "Enemy1A", 400, 375, 400, 2));
            enemies3.Add(new Enemy(1, "Enemy1A", 720, 375, 400, 2));
            enemies3.Add(new Enemy(1, "Enemy1A", 1000, 375, 400, 2));
            enemies3.Add(new Enemy(2, "Enemy1B", 1500, 700, 0, 3));
            enemies3.Add(new Enemy(1, "Enemy1A", 965, 720, 1500, 3));

            foreach (Enemy e in enemies1.ToArray())
            {
                e.LoadContent(Content);
            }

            foreach (Enemy e in enemies2.ToArray())
            {
                e.LoadContent(Content);
            }
            foreach (Enemy e in enemies3.ToArray())
            {
                e.LoadContent(Content);
            }
            //end enemy instantiation

            bullet = Content.Load<Texture2D>("Images/Objects/bullet");
        }

        public void Update(GameTime gameTime, int bgNum, List<Projectiles> bullets)
        {
            if (bgNum == 0)
            {
                foreach (Enemy e in enemies1.ToArray())
                {
                    e.Update(gameTime);
                    e.checkCollisions(bullets);
                    if (e.isDead())
                    {
                        enemies1.RemoveAt(enemies1.IndexOf(e));
                    }

                }
            }
            else if (bgNum == 1)
            {
                foreach (Enemy e in enemies2.ToArray())
                {
                    e.Update(gameTime);
                    e.checkCollisions(bullets);
                    if (e.isDead())
                    {
                        enemies2.RemoveAt(enemies2.IndexOf(e));
                    }
                }
            }
            else if (bgNum == 2)
            {
                foreach (Enemy e in enemies3.ToArray())
                {
                    e.Update(gameTime);
                    e.checkCollisions(bullets);
                    if (e.isDead())
                    {
                        enemies3.RemoveAt(enemies3.IndexOf(e));
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, int bgNum, int level)
        {
            if (bgNum == 0 && level == 1)
            {
                foreach (Enemy e in enemies1.ToArray())
                {
                    e.Draw(spriteBatch);
                }
            }
            else if (bgNum == 1 && level == 1)
            {
                foreach (Enemy e in enemies2.ToArray())
                {
                    e.Draw(spriteBatch);
                }
            }
            else if (bgNum == 2 && level == 1)
            {
                foreach (Enemy e in enemies3.ToArray())
                {
                    e.Draw(spriteBatch);
                }
            }
        }


    }
}
