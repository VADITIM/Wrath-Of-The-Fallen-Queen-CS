using Godot;

public partial class WallJump : Resource
{
    private Player Player;
    private PackedScene _abilityPopupScene;

    private bool wallJumping = false;
    private const float WALLJUMPDURATION = 1f;
    private Vector2 wallJumpStartVelocity;
    private Vector2 wallJumpTargetVelocity;
    private float wallJumpTimer;
    private Vector2 wallJumpStartPosition;
    private Vector2 wallJumpTargetPosition;
    private const float WALLJUMPDISTANCE = 100;
    private const float WALLJUMPPOWER = 9;
    private float jumpDirection;

    public void SetPlayer(Player player)
    {
        Player = player;
        _abilityPopupScene = GD.Load<PackedScene>("res://Scenes/AbilityPopup.tscn");
    }

    public void WallJumping()
    {
        Player.isClimbing = false;
        wallJumping = true;
        wallJumpTimer = 0f;

        if (Player._wallRaycastLeft.IsColliding())
        {
            jumpDirection = 1;
        }
        else if (Player._wallRaycastRight.IsColliding())
        {
            jumpDirection = -1;
        }
        else
        {
            return;
        }
        
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

        bool hitWall = (jumpDirection < 0 && Player._wallRaycastLeft.IsColliding()) ||
                       (jumpDirection > 0 && Player._wallRaycastRight.IsColliding());

        if (hitWall)
        {
            wallJumping = false;
            return;
        }
        
        wallJumpTimer += delta;
        float timer = wallJumpTimer / WALLJUMPDURATION;

        if (timer >= .3f)
        {
            wallJumping = false;
            return;
        }

        float easeOut = 1 - Mathf.Pow(1 - timer, WALLJUMPPOWER);
        Player.velocity = wallJumpStartVelocity.Lerp(wallJumpTargetVelocity, easeOut);
        Player.Position = wallJumpStartPosition.Lerp(wallJumpTargetPosition, easeOut);
    }
}
