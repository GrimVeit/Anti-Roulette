using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSectorHighlightView : View
{
    [SerializeField] private List<NumberSectorHighlight> numberSectorHighlightsList = new();
    [SerializeField] private float timeShowHide = 0.2f;

    private readonly Dictionary<int, NumberSectorHighlight> _numberSectors = new();
    private NumberSectorHighlight _activeHighlight;

    public void Initialize()
    {
        foreach (var numberSector in numberSectorHighlightsList)
        {
            // На старте всё спрятано
            numberSector.Hide(0f);
            _numberSectors[numberSector.Number] = numberSector;
        }
    }

    public void Show(int number)
    {
        // На всякий случай гасим предыдущий активный, если это другой номер
        if (_activeHighlight != null && _activeHighlight.Number != number)
        {
            _activeHighlight.Hide(timeShowHide);
        }

        if (!TryGetHighlight(number, out var highlight))
            return;

        _activeHighlight = highlight;
        highlight.Show(timeShowHide);
    }

    public void Hide(int number)
    {
        if (!TryGetHighlight(number, out var highlight))
            return;

        highlight.Hide(timeShowHide);

        if (_activeHighlight == highlight)
            _activeHighlight = null;
    }

    private bool TryGetHighlight(int number, out NumberSectorHighlight highlight)
    {
        if (_numberSectors.TryGetValue(number, out highlight))
            return true;

        Debug.LogWarning($"Not found NumberSectorHighlight with Number - {number}");
        highlight = null;
        return false;
    }
}
