using Godot;

public partial class DashUnlocker : Area2D
{
	public Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
	}

	private void OnBodyEntered(Player player)
	{
		if (player is Player)
		{
			player.Dash.UnlockDash();
			player.CollectUnlocker(this);
		}
	}
}
