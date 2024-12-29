using Godot;

public partial class Climbing : Resource
{
    private Player Player;
    private WallJump WallJump;
    private PackedScene _abilityPopupScene;

    private bool climbUnlocked = false;
    private float holdTimeout = 2f; 

    public void SetPlayer(Player player)
    {
        Player = player;
        WallJump = player.WallJump;
        _abilityPopupScene = GD.Load<PackedScene>("res://Scenes/AbilityPopup.tscn");
    }

    public void HandleClimb(float delta)
    {
        if (Player.canClimb && climbUnlocked)
        {
            if (Input.IsActionPressed("hold") && Player._wallRaycastLeft.IsColliding() && !Player.IsOnFloor() || Input.IsActionPressed("hold") && Player._wallRaycastRight.IsColliding() && !Player.IsOnFloor())
            {
                Player.isClimbing = true;
                Player.velocity = new Vector2(0, 0);
                holdTimeout -= delta;
                // Player._sprite.Play("climb");
                if (holdTimeout <= 0)
                {
                    Player.canClimb = false;
                    holdTimeout = 2f;
                }
                if (Input.IsActionPressed("up"))
                {
                    Player.velocity.Y = -Player.climbSpeed;
                }
                else if (Input.IsActionPressed("down"))
                {
                    Player.velocity.Y = Player.climbSpeed;
                }
                else if (Input.IsActionJustPressed("jump"))
                {
                    WallJump.WallJumping();
                    holdTimeout = 2f;
                }
            }
            else
                Player.isClimbing = false;
        }
        else
            Player.isClimbing = false;
    }

    public void UnlockClimb()
    {
        climbUnlocked = true;
        UnlockMessage();
    }

    private void UnlockMessage()
    {
        var abilityPopup = _abilityPopupScene.Instantiate<AbilityPopup>();
        Player.AddChild(abilityPopup);
        abilityPopup.ShowAbilityPopup(
            "Wall Climbing", 
            "You can now climb walls by holding the 'hold' button next to a wall. Use up and down to move vertically while climbing."
        );
    }
}