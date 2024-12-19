using Godot;

public partial class Jump : Resource
{
    private Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public void HandleJump()
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionJustPressed("jump") && Player.IsOnFloor() || Input.IsActionJustPressed("jump") && !Player.IsOnFloor())
            {
                // Player._sprite.Play("jump");
                Player.velocity.Y = Player.jumpForce;
            }
        }
        
        if (Player.IsOnCeiling())
        {
            Player.velocity.Y = 0;
        }
    }
}
