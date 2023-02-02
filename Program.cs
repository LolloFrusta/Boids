using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{
    class Program
    {
        public static Window Window = new Window(1280, 720, "Boids");
        public static List<Boid> Boids = new List<Boid>();
        static void Main(string[] args)
        {
            Texture BoidTexture = new Texture("Assets/boid.png");

            float SpawnTime = 0.25f;
            float counter = SpawnTime;

            while (Window.IsOpened)
            {
                counter -= Window.DeltaTime;
                //INPUT
                if (Window.MouseLeft)
                {
                    if (counter<=0)
                    {
                        Boid b = new Boid(BoidTexture);
                        b.Position = Window.MousePosition;
                        Boids.Add(b);
                        counter = SpawnTime;
                    }
                }
               

                if (Window.MouseRight)
                {
                    foreach (Boid boid in Boids)
                    {
                        Vector2 distVect = Window.MousePosition - boid.Position;

                        boid.Velocity = boid.Speed * distVect.Normalized();
                    }
                }
                if (Window.MouseMiddle)
                {
                    foreach (Boid boid in Boids)
                    {
                        Vector2 distVect = Window.MousePosition - boid.Position;

                        boid.Velocity = boid.Speed * (-distVect.Normalized());
                    }
                }


                //UPDATE
                foreach (Boid boid in Boids)
                {
                    boid.Update();
                }



                //DRAW
                foreach (Boid boid in Boids)
                {
                    boid.Draw();
                }



                Window.Update();
            }
        }
    }
}
