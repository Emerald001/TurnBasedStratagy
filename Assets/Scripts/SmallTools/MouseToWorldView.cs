using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToWorldView : MonoBehaviour
{
    private Hex lastTile;

    public Material Hovercolor;

    void Update()
    {
        Ray target = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(target, out hit, 10000)) {
            if (hit.transform.CompareTag("WalkableTile")) {
                var hitTile = hit.transform.parent.gameObject;

                if (hitTile != lastTile) {
                    if (lastTile != null)
                        lastTile.ResetColor();

                    lastTile = hitTile.GetComponent<Hex>();
                    lastTile.SetColor(Hovercolor);

                    MouseValues.HoverTileGridPos = UnitStaticFunctions.GetGridPosFromWorldPos(hitTile);
                }
            }
            else if(lastTile != null) {
                lastTile.ResetColor();
                MouseValues.HoverTileGridPos = Vector2Int.zero;
            }

            MouseValues.HoverPointPos = hit.point;
        }
    }
}