using Godot;

public partial class Climbing : Resource
{
    private Player Player;
    private PackedScene _abilityPopupScene;

    private bool climbUnlocked = false;

    private bool wallJumping = false;
    private const float WALLJUMPDURATION = 1f;
    private Vector2 wallJumpStartVelocity;
    private Vector2 wallJumpTargetVelocity;
    private float wallJumpTimer;
    private Vector2 wallJumpStartPosition;
    private Vector2 wallJumpTargetPosition;
    private const float WALLJUMPDISTANCE = 100;

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
                Player.isClimbing = true;
                Player.velocity = new Vector2(0, 0);
                // Player._sprite.Play("climb");

                if (Input.IsActionPressed("up"))
                {
                    Player.velocity.Y = -Player.climbSpeed;
                }
                else if (Input.IsActionPressed("down"))
                {
                    Player.velocity.Y = Player.climbSpeed;
                }
                else if (Input.IsActionPressed("jump"))
                {
                    WallJump();
                }
                else
                    Player.velocity.Y = 0;
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
        // UnlockMessage();
    }

    public void WallJump()
    {
        Player.isClimbing = false;
        wallJumping = true;
        wallJumpTimer = 0f;

        float jumpDirection = Player._sprite.FlipH ? -1 : 1;
        
        // Store start position and calculate target position
        wallJumpStartPosition = Player.Position;
        wallJumpTargetPosition = wallJumpStartPosition + new Vector2(
            jumpDirection * WALLJUMPDISTANCE,  // Horizontal distance
            -WALLJUMPDISTANCE                  // Vertical distance (negative for upward)
        );
    }
    public void HandleWallJump(float delta)
    {
        if (!wallJumping) return;

        wallJumpTimer += delta;
        float timer = wallJumpTimer / WALLJUMPDURATION;

        if (timer >= .3f)
        {
            wallJumping = false;
            return;
        }

        float easeOut = 1 - Mathf.Pow(1 - timer, 3);
        Player.velocity = wallJumpStartVelocity.Lerp(wallJumpTargetVelocity, easeOut);
        Player.Position = wallJumpStartPosition.Lerp(wallJumpTargetPosition, easeOut);
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