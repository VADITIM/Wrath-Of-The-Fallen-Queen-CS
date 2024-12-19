using Godot;

public partial class ClimbUnlocker : Area2D
{
    public Sprite2D _sprite;
    private Float Float = new Float();

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        Float.SetOwner(this);
        Float.Floating();
    }

    private void OnBodyEntered(Player player)
    {
        Float.StopFloating();
        player.Climbing.UnlockClimb();
        player.CollectUnlocker(this);
    }

}