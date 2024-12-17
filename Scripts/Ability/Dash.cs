using Godot;
using System.Collections.Generic;

public partial class Dash : Resource
{
    private Player Player;
    private PackedScene _abilityPopupScene;
    
    public float dashCooldown = 1.6f;
    public bool canDash = true;
    public bool isDashing = false;
    public float currentDashCooldown = 0f;
    public float dashDuration = .2f;
    public bool dashUnlocked = false;
    public float ghostDashInterval = 0f; 
    private float ghostDashElapsedTime = 0f; 

    public void SetPlayer(Player player)
    {
        Player = player;
        _abilityPopupScene = GD.Load<PackedScene>("res://Scenes/AbilityPopup.tscn");
    }

    public void HandleDash(float delta)
    {
        if (!canDash && dashUnlocked)
        {
            currentDashCooldown += delta;
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

        Player._sprite.Play("dash");
        isDashing = true;
        canDash = false;
        currentDashCooldown = 0f;
        ghostDashElapsedTime = 0f;
        GD.Print("Dash started");

        Player._dashTimer.WaitTime = dashDuration;
        Player._dashTimer.Start();
    }

    public void UnlockDash()
    {
        dashUnlocked = true;
        Player._dashIndicator.Visible = true;
        UnlockMessage();
    }

    private void UnlockMessage()
    {
        var abilityPopup = _abilityPopupScene.Instantiate<AbilityPopup>();
        Player.AddChild(abilityPopup);
        abilityPopup.ShowAbilityPopup(
            "Dashing", 
            "You can now dash by pushing the [F] key."
        );
    }
}