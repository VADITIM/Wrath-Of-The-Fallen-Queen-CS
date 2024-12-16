using Godot;

public partial class Movement : Resource
{
    private Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public void HandleMovement()
    {
        if (!Player.isClimbing)
        {
            if (Player.Dash.isDashing)
            {
                Player._sprite.Play("dash");
                return;
            }

            Player.velocity.X = 0;
            if (Input.IsActionPressed("left"))
            {
                Player.velocity.X = -Player.speed;
                Player._sprite.Play("move");
            }
            else if (Input.IsActionPressed("right"))
            {
                Player.velocity.X = Player.speed;
                Player._sprite.Play("move");
            }
            else 
                Player._sprite.Play("idle");
        }
    }
}
