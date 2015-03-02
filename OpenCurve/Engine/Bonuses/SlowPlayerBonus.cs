namespace OpenCurve.Engine.Bonuses
{
    public class SlowPlayerBonus : IPlayerBonus
    {
        public void Apply(Player player)
        {
            player.MoveSpeed = player.MoveSpeed*0.75f;
        }
    }
}
