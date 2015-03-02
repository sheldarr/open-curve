﻿namespace OpenCurve.Engine.Bonuses
{
    public class SpeedPlayerBonus : IPlayerBonus
    {
        public void Apply(Player player)
        {
            player.MoveSpeed = player.MoveSpeed*1.5f;
        }
    }
}
