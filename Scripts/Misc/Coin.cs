using Godot;

public partial class Coin : Area2D
{
    public Sprite2D _sprite;

    private float coinCount = 0;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");

        Float();
    }

    private void Float()
    {
        Vector2 startPosition = Position;
        Vector2 endPosition = Position + new Vector2(0, -10); 

        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", endPosition, 1.0f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(this, "position", startPosition, 1.0f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.SetLoops();
    }

    private void OnBodyEntered(Player player)
    {
        if (player is Player)
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        QueueFree();
        CoinIncrement();
        GD.Print("Coin collected!");
    }

    private void CoinIncrement()
    {
        coinCount++;
        GD.Print("Coin count: " + coinCount);
    }
}