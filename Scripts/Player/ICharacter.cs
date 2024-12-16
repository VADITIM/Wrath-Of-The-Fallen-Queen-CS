using Godot;

public interface ICharacter
{
    Vector2 Velocity { get; set; }
    bool IsOnFloor();
    void PlayAnimation(string animation);
}