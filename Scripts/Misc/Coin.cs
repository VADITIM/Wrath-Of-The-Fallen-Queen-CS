using Godot;

public partial class Coin : Area2D
{
    public Sprite2D _sprite;
	private Float Float = new Float();

    private float coinCount = 0;

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
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        Float.StopFloating();
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