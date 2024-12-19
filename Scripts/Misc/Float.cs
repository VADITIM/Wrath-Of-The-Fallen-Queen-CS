using Godot;

public partial class Float : Resource
{
    private Area2D _owner;
    private Tween tween;

    public void SetOwner(Area2D owner)
    {
        _owner = owner;
    }

    public void Floating()
    {
        if (_owner == null)
        {
            return;
        }

        Vector2 startPosition = _owner.Position;
        Vector2 endPosition = startPosition + new Vector2(0, -10);

        tween = _owner.GetTree().CreateTween();

        tween.TweenProperty(_owner, "position", endPosition, 1.0f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(_owner, "position", startPosition, 1.0f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.SetLoops();
    }

    public void StopFloating()
    {
        tween.Stop();
    }
}