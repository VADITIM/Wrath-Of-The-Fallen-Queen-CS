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
    private Area2D _hurtbox;

    [Export] public PackedScene _ghostDash;
    // ----------------------------------------------------

    // SCRIPTS  --------------------------------------------------------------------------------------------------------
    private Movement Movement = new Movement();
    public Climbing Climbing = new Climbing();
    private Gravity Gravity = new Gravity();
    private Jump Jump = new Jump();
    public Dash Dash = new Dash();
    private Coin Coin = new Coin();
    private DashUnlocker DashUnlocker = new DashUnlocker();
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
        // var raycastsNode = GetNode<Node>("Raycasts");
        _wallRaycastLeft = GetNode<RayCast2D>("WallRaycastLeft");
        _wallRaycastRight = GetNode<RayCast2D>("WallRaycastRight");
        _damageLayer = GetNode<TileMapLayer>("/root/World/DamageLayer");
        _iFrames = GetNode<Timer>("IFrames");
        _dashTimer = GetNode<Timer>("DashTimer");
        _dashIndicator = GetNode<Sprite2D>("DashIndicator");
        _hurtbox = GetNode<Area2D>("Hurtbox");
        // ----------------------------------------------------

        // SCRIPTS  --------------------------------------------------------------------------------------------------------
        Movement.SetPlayer(this);
        Climbing.SetPlayer(this);
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
        Climbing.HandleClimb();

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

    private void OnHurtboxAreaEntered(Area2D area)
    {
        CheckForDamage();
    }

    // TRY WITH ISONCEILING
    private void CheckForDamage()
    {
        if (damageTaken) return;

        // Get the current tile position of the player
        Vector2I currentTilePos = _damageLayer.LocalToMap(GlobalPosition);
        
        // Check the tile at player's position and surrounding tiles
        Vector2I[] checkPositions = new Vector2I[]
        {
            currentTilePos,                    // Current position
            currentTilePos + new Vector2I(0, 1),  // Below
            currentTilePos + new Vector2I(0, -1), // Above
            currentTilePos + new Vector2I(1, 0),  // Right
            currentTilePos + new Vector2I(-1, 0)  // Left
        };

        foreach (var pos in checkPositions)
        {
            if (_damageLayer.GetCellSourceId(pos) != -1)
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
