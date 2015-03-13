namespace OpenCurve.Engine.Factories
{
    public static class PlayerFactory
    {
        public static Player CreatePlayer(PlayerOptions playerOptions)
        {
            return new Player
            {
                Color = playerOptions.Color,
                MoveLeftKey = playerOptions.MoveLeftKey,
                MoveRightKey = playerOptions.MoveRightKey,
                PadController = playerOptions.PadController,
                PlayerIndex = playerOptions.PlayerIndex
            };
        }
    }
}
