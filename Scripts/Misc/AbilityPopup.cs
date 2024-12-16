using Godot;
using System;

public partial class AbilityPopup : Control
{
    private Panel _popupPanel;
    private Label _abilityNameLabel;
    private Label _descriptionLabel;
    private Button _closeButton;

    public override void _Ready()
    {
        // Find the necessary nodes
        _popupPanel = GetNode<Panel>("PopupPanel");
        _abilityNameLabel = GetNode<Label>("PopupPanel/Name");
        _descriptionLabel = GetNode<Label>("PopupPanel/Description");
        _closeButton = GetNode<Button>("PopupPanel/CloseButton");

        _closeButton.Pressed += OnCloseButtonPressed;

        _popupPanel.Visible = false;
    }

    public void ShowAbilityPopup(string abilityName, string description)
    {
        _abilityNameLabel.Text = abilityName;
        _descriptionLabel.Text = description;

        _popupPanel.Visible = true;

        // Optional: Pause the game while popup is shown
        // GetTree().Paused = true;
    }

    private void OnCloseButtonPressed()
    {
        _popupPanel.Visible = false;
        GetTree().Paused = false;
    }
}