using System;
using Godot;

public partial class Player : CharacterBody2D
{
    // NODES    --------------------------------------------------------------------------------------------------------
    public AnimatedSprite2D _sprite;
    public RayCast2D _wallRaycastLeft;
    public RayCast2D _wallRaycastRight;
    public TileMapLayer _damageLayer;
    private Timer _iFrames;
    public Timer _dashTimer;
    public Sprite2D _dashIndicator;

    [Export] public PackedScene _ghostDash;
    // ----------------------------------------------------

    // SCRIPTS  --------------------------------------------------------------------------------------------------------
    public Movement Movement = new Movement();
    private Jump Jump = new Jump();
    public Dash Dash = new Dash();
    public Climbing Climbing = new Climbing();
    public WallJump WallJump = new WallJump();
    private Gravity Gravity = new Gravity();
    // ----------------------------------------------------
    

    [Export] public float speed = 600f;
    [Export] public float jumpForce = -800f;
    [Export] public float gravity = 30f;
    [Export] public float dashSpeed = 900f;
    [Export] public float climbSpeed = 100f;
    [Export] public int health = 3;
    private bool damageTaken = false;

    public Vector2 velocity = Vector2.Zero;

    public bool canClimb = true;
    public bool isClimbing = false;
    

    public override void _Ready()
    {
        // NODES    --------------------------------------------------------------------------------------------------------
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _wallRaycastLeft = GetNode<RayCast2D>("Raycasts/WallRaycastLeft");
        _wallRaycastRight = GetNode<RayCast2D>("Raycasts/WallRaycastRight");
        _damageLayer = GetNode<TileMapLayer>("/root/World/Layers/DamageLayer");
        _iFrames = GetNode<Timer>("Timers/IFrames");
        _dashTimer = GetNode<Timer>("Timers/DashTimer");
        _dashIndicator = GetNode<Sprite2D>("DashIndicator");
        // ----------------------------------------------------

        // SCRIPTS  --------------------------------------------------------------------------------------------------------
        Movement.SetPlayer(this);
        Climbing.SetPlayer(this);
        WallJump.SetPlayer(this);
        Gravity.SetPlayer(this);
        Jump.SetPlayer(this);
        Dash.SetPlayer(this);
        // ----------------------------------------------------

        _dashIndicator.Visible = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement.HandleMovement();
        Jump.HandleJump();

        Gravity.HandleGravity();
        Climbing.HandleClimb((float) delta);
        WallJump.HandleWallJump((float)delta);

        Dash.HandleDash((float)delta);
        DisplayGhost();
        
        FlipSprite();
        CheckIfOnFloor();
        CheckForDamage();   

        MoveAndSlide();
    }

    private void DisplayGhost()
    {
        if (!Dash.isDashing) return;
        GhostDash ghost = _ghostDash.Instantiate() as GhostDash;
        Owner.AddChild(ghost);
        ghost.GlobalPosition = this.GlobalPosition;
        ghost.FlipSprite(_sprite.FlipH);
    }

    public void CollectUnlocker(Node collectedObject)
    {
        collectedObject.QueueFree();
    }

    private void CheckIfOnFloor()
    {
        if (IsOnFloor())
        {
            canClimb = true;
        }
    }

    private void ApplyDamage()
    {
        if (damageTaken) return;

        health--;
        damageTaken = true;
        _iFrames.Start();
        
        // Optional: Add visual feedback
        _sprite.Modulate = new Color(1, 0.3f, 0.3f, 0.7f); // Red tint
        
        GD.Print("Player health: " + health);
    }

    private void CheckForDamage()
    {
        int tileSize = 20;
        
        if (damageTaken) return;

        // Get the current tile position of the player
        Vector2I currentTilePos = _damageLayer.LocalToMap(GlobalPosition);
        Vector2I playerPos = _damageLayer.LocalToMap(GlobalPosition + new Vector2(0, -tileSize));

        // Check the tile at player's position and surrounding tiles
        Vector2I[] checkPositions = new Vector2I[]
        {
            currentTilePos,      
            playerPos,
            playerPos + new Vector2I(0, 1),  // Below
            playerPos + new Vector2I(0, -1), // Above
        };

        foreach (Vector2I pos in checkPositions)
        {
            int sourceId = _damageLayer.GetCellSourceId(pos);
            if (sourceId != -1)
            {
                ApplyDamage();
                return;
            }
        }
    }

    private void FlipSprite()
    {
        if (Input.IsActionPressed("left"))
            _sprite.FlipH = true;
        else if (Input.IsActionPressed("right"))
            _sprite.FlipH = false;
        Velocity = velocity;
    }

    // SIGNALS  --------------------------------------------------------------------------------------------------------
    private void OnIFramesTimeout()
    {
        GD.Print("IFrames ended");
        damageTaken = false;
        _sprite.Modulate = new Color(1, 1, 1, 1); // Reset color
    }
    
    public void OnDashTimeout()
    {
        Dash.isDashing = false;
        velocity.X = 0;
        Dash.currentDashCooldown = 0f;
    }
}
