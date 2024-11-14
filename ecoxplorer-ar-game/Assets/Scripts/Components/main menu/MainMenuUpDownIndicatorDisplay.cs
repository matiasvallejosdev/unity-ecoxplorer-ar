using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;

public class MainMenuUpDownIndicatorDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleScrollSnap scrollSnap;
    [SerializeField] private RawImage upIndicator;
    [SerializeField] private RawImage downIndicator;

    private void Start()
    {
        if (scrollSnap != null)
        {
            scrollSnap.OnPanelCentered.AddListener(OnPanelChanged);
            UpdateIndicators(scrollSnap.CenteredPanel);
        }
        downIndicator.enabled = true;
    }

    private void OnPanelChanged(int newPanel, int oldPanel)
    {
        UpdateIndicators(newPanel);
    }

    private void UpdateIndicators(int currentPanel)
    {
        // Show/hide up indicator
        if (upIndicator != null)
        {
            upIndicator.enabled = currentPanel > 0;
        }

        // Show/hide down indicator
        if (downIndicator != null)
        {
            downIndicator.enabled = currentPanel < scrollSnap.NumberOfPanels - 1;
        }
    }

    private void OnDestroy()
    {
        if (scrollSnap != null)
        {
            scrollSnap.OnPanelCentered.RemoveListener(OnPanelChanged);
        }
    }
}