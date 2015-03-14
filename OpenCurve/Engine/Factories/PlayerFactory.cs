namespace OpenCurve.Engine.Factories
{
    public static class PlayerFactory
    {
        public static Player CreatePlayer(PlayerOptions playerOptions)
        {
            return new Player
            {
                Color = playerOptions.Color,
                PlayerControls = playerOptions.PlayerControls
            };
        }
    }
}
