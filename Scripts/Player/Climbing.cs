using Godot;

public partial class Climbing : Resource
{
    private Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
    }


    public void HandleClimb()
    {
        if (Player.canClimb)
        {
            if (Input.IsActionPressed("shoot") && Player._wallRaycastLeft.IsColliding() && !Player.IsOnFloor() || Input.IsActionPressed("shoot") && Player._wallRaycastRight.IsColliding() && !Player.IsOnFloor())
            {
                Player.isClimbing = true;
                Player.velocity = new Vector2(0, 0);
                Player._sprite.Play("climb");

                if (Input.IsActionPressed("up"))
                {
                    Player.velocity.Y = -Player.climbSpeed;
                }
                else if (Input.IsActionPressed("down"))
                {
                    Player.velocity.Y = Player.climbSpeed;
                }
                else
                {
                    Player.velocity.Y = 0;
                }
            }
            else
                Player.isClimbing = false;
        }
        else
            Player.isClimbing = false;
    }
}