using TMPro;
using UnityEngine;

namespace ImprovedClock;

public class SpectatorClock : MonoBehaviour {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public CanvasGroup canvasGroup;

    public TextMeshProUGUI clockNumber;
    public UnityEngine.UI.Image clockIcon;

    public UnityEngine.UI.Image clockBox;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private void Start() {
        SetClock();
        ImprovedClock.SetClockColor();

        SetClockVisibility();
    }

    public void SetClockVisibility() => canvasGroup.alpha = ConfigManager.clockVisibilityInSpectator.Value;

    public void SetClock() {
        clockNumber.text = HUDManager.Instance.clockNumber.text;
        clockIcon.sprite = HUDManager.Instance.clockIcon.sprite;
    }

    public void SetClockVisible(bool visible) {
        ImprovedClock.Logger.LogDebug($"Toggling SpectatorClock visibility! Visible? {visible}");

        canvasGroup.alpha = visible? ConfigManager.clockVisibilityInSpectator.Value : 0F;
        canvasGroup.enabled = visible;

        clockNumber.enabled = visible;
        clockIcon.enabled = visible;
        clockBox.enabled = visible;
    }
}