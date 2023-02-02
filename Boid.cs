using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Fast2D;

namespace Boids
{
    class Boid
    {
        Sprite sprite;
        Texture texture;

        private float cohesionRadius;
        private float alignmentRadius;
        private float separationRadius;
        private float colorRadius;
        private float blend;
        private float VisionAngle = MathHelper.DegreesToRadians(250);


        public Sprite Sprite { get { return sprite; } }


        public Vector2 Position { get { return sprite.position; } set { sprite.position = value; } }

        public Vector2 Forward
        {
            get { return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation)); }
            set { sprite.Rotation = (float)Math.Atan2(value.Y, value.X); }
        }

        public Vector4 Color;
        public Vector2 Velocity;
        public int Speed = 150;

        public Boid(Texture texture)
        {
            Color = new Vector4(RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomFloat(), RandomGenerator.GetRandomFloat(), 0);

            this.texture = texture;
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.scale = new Vector2(0.8f);
            sprite.SetAdditiveTint(Color);

            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            Vector2 dir = new Vector2(RandomGenerator.GetRandomInt(int.MinValue, int.MaxValue), RandomGenerator.GetRandomInt(int.MinValue, int.MaxValue));
            Velocity = dir.Normalized() * Speed;
            Forward = dir.Normalized();

            colorRadius = cohesionRadius = 120;
            alignmentRadius = 90;
            separationRadius = 60;
            blend = 0.2f;
        }

        public void Update()
        {
            if (Velocity != Vector2.Zero)
            {
                Forward = Velocity;
            }

            Vector2 v = GetAverage();
            Vector4 vColor = AverageColor();
            if (v != Vector2.Zero && vColor != Vector4.Zero)
            {                
                Vector2 dir = Vector2.Lerp(Forward, v, blend);
                Forward = dir.Normalized(); ;
                Velocity = dir * Speed;
                sprite.SetAdditiveTint(vColor);
            }

            CheckPosition();
            sprite.position += Velocity * Program.Window.DeltaTime;            
        }

        public Vector2 GetAverage()
        {
            Vector2 vAlignment = AverageAlignment();
            Vector2 vCohesion = AverageCohesion();
            Vector2 vSeparation = AverageSeparation();

            return (vAlignment * 0.15f + vCohesion * 0.15f + vSeparation * 0.7f) / 3;

        }

        public Vector2 AverageAlignment()
        {
            List<Boid> nearBoids = GetNearBoids(alignmentRadius);
            Vector2 vectorSum = Vector2.Zero;

            if (nearBoids.Count != 0)
            {
                for (int i = 0; i < nearBoids.Count; i++)
                {
                    //vectorSum = Vector2.Add(vectorSum, nearBoids[i].Forward);
                    vectorSum += nearBoids[i].Forward;
                }
                vectorSum /= nearBoids.Count;
                return vectorSum.Normalized();
            }
            return Vector2.Zero;
        }

        public Vector2 AverageCohesion()
        {
            List<Boid> nearBoids = GetNearBoids(cohesionRadius);
            Vector2 vectorSum = Vector2.Zero;

            if (nearBoids.Count != 0)
            {
                for (int i = 0; i < nearBoids.Count; i++)
                {
                    vectorSum = Vector2.Add(vectorSum, nearBoids[i].Position - Position);

                    //vectorSum += nearBoids[i].Position;
                }
                vectorSum /= nearBoids.Count;
                return vectorSum.Normalized();
            }
            return Vector2.Zero;
        }

        public Vector2 AverageSeparation()
        {
            List<Boid> nearBoids = GetNearBoids(separationRadius);
            Vector2 vectorSum = Vector2.Zero;

            if (nearBoids.Count != 0)
            {
                for (int i = 0; i < nearBoids.Count; i++)
                {
                    vectorSum = Vector2.Add(vectorSum, nearBoids[i].Position - Position);
                    //vectorSum += nearBoids[i].Position - Position;
                }
                vectorSum /= nearBoids.Count;

                return -vectorSum.Normalized();
            }
            return Vector2.Zero;
        }

        public Vector4 AverageColor()
        {
            List<Boid> nearBoids = GetNearBoids(colorRadius);
            Vector4 vectorSum = Vector4.Zero;

            if (nearBoids.Count != 0)
            {
                for (int i = 0; i < nearBoids.Count; i++)
                {
                    vectorSum += nearBoids[i].Color;
                }
                vectorSum /= nearBoids.Count;
                return vectorSum;
            }
            return Vector4.Zero;
        }

        public List<Boid> GetNearBoids(float VisionRadius)
        {
            List<Boid> boids = Program.Boids;
            List<Boid> nearBoids = new List<Boid>();

            for (int i = 0; i < boids.Count; i++)
            {

                Vector2 distVect = boids[i].Position - Position;

                if (distVect.LengthSquared <= VisionRadius * VisionRadius)
                {
                    float playerPosAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1.0f, 1.0f));
                    if (playerPosAngle <= VisionAngle * 0.5f)
                    {
                        nearBoids.Add(boids[i]);
                    }
                }
            }
            return nearBoids;
        }

        public void Draw()
        {
            sprite.DrawTexture(texture);
        }

        public void CheckPosition()
        {
            //SINISTRA
            if (Sprite.position.X + Sprite.pivot.X < 0)
            {
                sprite.position.X = Program.Window.Width;
            }
            //DESTRA
            if (Sprite.position.X - Sprite.pivot.X > Program.Window.Width)
            {
                sprite.position.X = -Sprite.pivot.X;
            }
            //ALTO
            if (Sprite.position.Y + Sprite.pivot.Y < 0)
            {
                sprite.position.Y = Program.Window.Height;
            }
            //BASSO
            if (Sprite.position.Y - Sprite.pivot.X > Program.Window.Height)
            {
                sprite.position.Y = -Sprite.pivot.Y;
            }

        }
    }
}
