using Godot;

public partial class DashUnlocker : Area2D
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
		if (player is Player)
		{
			Float.StopFloating();
			player.Dash.UnlockDash();
			player.CollectUnlocker(this);
		}
	}
}
