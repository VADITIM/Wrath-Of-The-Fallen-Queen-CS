using Godot;

public partial class Gravity : Resource
{
    private Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public void HandleGravity()
    {
        if (!Player.isClimbing && !Player.IsOnFloor())
            Player.velocity.Y += Player.gravity;
    }
}