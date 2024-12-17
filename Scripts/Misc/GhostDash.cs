using Godot;

public partial class GhostDash : Node2D
{
	private AnimationPlayer _animationPlayer;
	public Sprite2D _sprite;
	
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer.Play("FadeOut");
	}
	public void FlipSprite(bool flipH)
	{
		_sprite.FlipH = flipH;
	}
}
