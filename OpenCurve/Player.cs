﻿namespace OpenCurve
{
    using System;
    using System.Collections.Generic;
    using Engine.Bonuses;
    using Microsoft.Xna.Framework;

    public class Player
    {
        public Color Color { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }

        public float BasicMoveSpeed { get; set; }
        public float BasicRotationSpeed { get; set; }
        public int BasicSize { get; set; }

        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public int Size { get; set; }

        public bool IsAlive { get; set; }

        public List<IPlayerBonus> PlayerBonuses { get; set; } 

        public Player()
        {
            Color = Color.Red;

            Position = new Vector2(0, 0);
            Direction = new Vector2(1, 1);

            BasicMoveSpeed = 1.0f;
            BasicRotationSpeed = 0.1f;
            BasicSize = 4;

            IsAlive = true;
        }

        public void ApplyBonuses()
        {
            MoveSpeed = BasicMoveSpeed;
            RotationSpeed = BasicRotationSpeed;
            Size = BasicSize;

            PlayerBonuses.ForEach(pb => pb.Apply(this));
        }

        public void MakeMove()
        {
            var positionOffsetX = Direction.X*MoveSpeed;
            var positionOffsetY = Direction.Y*MoveSpeed;

            var positionOffsetVector = new Vector2(positionOffsetX, positionOffsetY);

            Position += positionOffsetVector;
        }

        public void TurnLeft()
        {
            Direction = Vector2.Normalize(CalculateNewDirection(-BasicRotationSpeed));
        }

        public void TurnRight()
        {
            Direction = Vector2.Normalize(CalculateNewDirection(BasicRotationSpeed));
        }

        private Vector2 CalculateNewDirection(float angle)
        {
            var newDirectionX = (float)((Direction.X * Math.Cos(angle)) - (Direction.Y * Math.Sin(angle)));
            var newDirectionY = (float)((Direction.Y * Math.Cos(angle)) + (Direction.X * Math.Sin(angle)));

            return new Vector2(newDirectionX, newDirectionY);
        }
    }
}
