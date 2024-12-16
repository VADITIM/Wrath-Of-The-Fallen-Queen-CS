using Godot;

public partial class Climbing : Resource
{
    private Player Player;
    private PackedScene _abilityPopupScene;

    private bool climbUnlocked = false;

    public void SetPlayer(Player player)
    {
        Player = player;
        _abilityPopupScene = GD.Load<PackedScene>("res://Scenes/AbilityPopup.tscn");
    }

    public void HandleClimb()
    {
        if (Player.canClimb && climbUnlocked)
        {
            if (Input.IsActionPressed("hold") && Player._wallRaycastLeft.IsColliding() && !Player.IsOnFloor() || Input.IsActionPressed("hold") && Player._wallRaycastRight.IsColliding() && !Player.IsOnFloor())
            {
                GD.Print("Climbing");
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