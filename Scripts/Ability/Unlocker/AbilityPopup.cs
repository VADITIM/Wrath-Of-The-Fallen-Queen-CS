using Godot;
using System;

public partial class AbilityPopup : Control
{
    private Panel _popupPanel;
    private Label _abilityNameLabel;
    private Label _descriptionLabel;
    private Button _closeButton;
    public AnimationPlayer _animationPlayer;

    private float popDownDuration = 0.3f;

    public override void _Ready()
    {
        // Find the necessary nodes
        _popupPanel = GetNode<Panel>("PopupPanel");
        _abilityNameLabel = GetNode<Label>("PopupPanel/Name");
        _descriptionLabel = GetNode<Label>("PopupPanel/Description");
        _closeButton = GetNode<Button>("PopupPanel/CloseButton");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _closeButton.Pressed += OnCloseButtonPressed;

        _popupPanel.Visible = false;
    }

    public void ShowAbilityPopup(string abilityName, string description)
    {
        _animationPlayer.Play("PopUp");
        _abilityNameLabel.Text = abilityName;
        _descriptionLabel.Text = description;

        _popupPanel.Visible = true;

        // Optional: Pause the game while popup is shown
        // GetTree().Paused = true;
    }

    private void OnCloseButtonPressed()
    {
        GD.Print("Close button pressed");
        _animationPlayer.Play("PopDown");
    }

    private void OnAnimationFinished(string animationname)
    {
        if (animationname == "PopDown")
        {
            _popupPanel.Visible = false;
            GetTree().Paused = false;
        }
    }
}