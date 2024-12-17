using Godot;

public partial class ClimbUnlocker : Area2D
{
    public Sprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }

    private void OnBodyEntered(Player player)
    {
        player.Climbing.UnlockClimb();
        player.CollectUnlocker(this);
    }

}