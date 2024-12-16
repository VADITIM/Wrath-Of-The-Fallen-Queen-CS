using Godot;


public partial class Dash : Resource
{
    private Player Player;

    public void SetPlayer(Player player)
    {
        Player = player;
    }


    public float dashCooldown = 2f;
    public bool canDash = true;
    public bool isDashing = false;
    public bool dashUnlocked = false;
    public double currentDashCooldown = 0f;
    public float dashDuration = .2f;

    public void UnlockDash()
    {
        dashUnlocked = true;
        Player._dashIndicator.Visible = true;
    }


    public void HandleDash()
    {
        if (!canDash && dashUnlocked)
        {
            currentDashCooldown += Player.GetProcessDeltaTime();
            Player._dashIndicator.Visible = false;
            if (currentDashCooldown >= dashCooldown)
            {
                canDash = true;
                currentDashCooldown = 0f;
                GD.Print("DASH REAAAAAAAAAAAAAAAAAADY");
                Player._dashIndicator.Visible = true;
            }
        }

        if (Input.IsActionJustPressed("dash") && canDash && dashUnlocked)
        {
            StartDash();
        }

        if (isDashing)
        {
            float dashDirection = Player._sprite.FlipH ? -1 : 1;
            Player.velocity.X = Player.dashSpeed * dashDirection;
        }
    }

    private void StartDash()
    {
        if (!canDash) return;
        
        isDashing = true;
        canDash = false;
        currentDashCooldown = 0f;
        GD.Print("Dash started");

        Player._dashTimer.WaitTime = dashDuration;
        Player._dashTimer.Start();
    }
}