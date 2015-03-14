namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class PlayerControls
    {
        public Keys MoveLeftKey { get; set; }
        public Keys MoveRightKey { get; set; }

        public bool PadController { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
    }
}
