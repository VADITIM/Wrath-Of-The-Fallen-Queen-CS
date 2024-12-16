using Godot;

public partial class Jumppad : Area2D
{
    private Sprite2D _sprite;
    private Player _playerOnJumppad;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }

    private void OnBodyEntered(Player body)
    {
        if (body is Player player)
        {
            GD.Print("Player entered jumppad");
            _playerOnJumppad = player;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player player && player == _playerOnJumppad)
        {
            GD.Print("Player exited jumppad");
            _playerOnJumppad = null;
        }
    }

    public override void _Process(double delta)
    {
        if (_playerOnJumppad != null && Input.IsActionJustPressed("jump"))
        {
            GD.Print("Player jumped on jumppad");
            _playerOnJumppad.velocity.Y = _playerOnJumppad.jumpForce * 1.4f;
        }
    }
}