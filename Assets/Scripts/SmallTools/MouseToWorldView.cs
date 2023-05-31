using UnityEngine;

public class MouseToWorldView : MonoBehaviour
{
    [SerializeField] private Material Hovercolor;
    private Hex lastTile;

    void Update() {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 10000))
            return;

        if (!hit.transform.CompareTag("WalkableTile")) {
            if (lastTile != null) {
                lastTile.ResetColor();
                MouseValues.HoverTileGridPos = Vector2Int.zero;
            }

            MouseValues.HoverPointPos = hit.point;
            return;
        }

        GameObject hitTile = hit.transform.parent.gameObject;
        if (hitTile != lastTile) {
            lastTile?.ResetColor();
            lastTile = hitTile.GetComponent<Hex>();
            lastTile.SetColor(Hovercolor);

            MouseValues.HoverTileGridPos = UnitStaticFunctions.GetGridPosFromWorldPos(hitTile);
        }

        MouseValues.HoverPointPos = hit.point;
    }
}