using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    //singleton bad, must change whenever possible
    public static Tooltip instance;
    private void Awake() {
        instance = this;

        tooltipText = GetComponentInChildren<Text>();
        backgroundRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update() {
        var parentRect = transform.parent.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, null, out localPoint);

        if (localPoint.x + backgroundRectTransform.rect.width > parentRect.rect.width / 2)
            localPoint.x = parentRect.rect.width / 2 - backgroundRectTransform.rect.width;
        if (localPoint.y + backgroundRectTransform.rect.height > parentRect.rect.height / 2)
            localPoint.y = parentRect.rect.height / 2 - backgroundRectTransform.rect.height;

        transform.localPosition = localPoint;
    }

    public void ShowTooltip(string tooltipString) {
        tooltipText.text = tooltipString;
        float textpadding = 10f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textpadding * 2f, tooltipText.preferredHeight + textpadding * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;

        gameObject.SetActive(true);
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString) {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static() {
        instance.HideTooltip();
    }
}