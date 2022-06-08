using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TextToShow;

    public void OnPointerEnter(PointerEventData eventData) {
        if (TextToShow != "")
            Tooltip.ShowTooltip_Static(TextToShow);
    }
    public void OnPointerExit(PointerEventData eventData) {
        Tooltip.HideTooltip_Static();
    }
}