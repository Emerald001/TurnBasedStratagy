using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitComponents;

public class UnitInfoPiece : MonoBehaviour
{
    public TurnManager turnManager;

    public float moveSpeed;

    public Vector2 UpPos;
    private Vector3 startingPos;

    public GameObject Panel;
    private RectTransform panelTranform;
    public Image Icon;
    public Text Name;
    public Text Stats;

    public List<Image> AbilityImages = new List<Image>();
    public List<Image> EffectImages = new List<Image>();

    private bool goUp = false;

    private void Awake() {
        panelTranform = Panel.GetComponent<RectTransform>();
        startingPos = panelTranform.localPosition;
    }

    private void Update() {
        if (goUp) {
            panelTranform.localPosition = Vector3.Lerp(panelTranform.localPosition, UpPos, moveSpeed * Time.deltaTime);
        }
        else {
            panelTranform.localPosition = Vector3.Lerp(panelTranform.localPosition, startingPos, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            if (goUp) {
                goUp = false;
                return;
            }

            var unit = UnitStaticFunctions.GetUnitFromGridPos(MouseValues.HoverTileGridPos);
            if (unit) {
                SetInfoPiece(unit);
                goUp = true;
            }
        }
    }

    public void SetInfoPiece(UnitManager unit) {
        var values = unit.values;
        
        Icon.sprite = unit.Icon;
        Name.text = unit.name;
        Stats.text = GetInfo(values);

        for (int i = 0; i < AbilityImages.Count; i++) {
            if(unit.abilities.Count > i && unit.abilities.Count != 0) {
                AbilityImages[i].gameObject.SetActive(true);
                AbilityImages[i].sprite = unit.abilities[i].Icon;
                AbilityImages[i].GetComponent<ToolTipTrigger>().TextToShow = unit.abilities[i].Description;
            }
            else {
                AbilityImages[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < EffectImages.Count; i++) {
            if (values.Effects.Count > i && values.Effects.Count != 0) {
                EffectImages[i].gameObject.SetActive(true);
                EffectImages[i].sprite = values.Effects[i].Icon;
                EffectImages[i].GetComponent<ToolTipTrigger>().TextToShow = values.Effects[i].Description;
            }
            else {
                EffectImages[i].gameObject.SetActive(false);
            }
        }
    }

    public string GetInfo(UnitValues unit) {
        string endString = "";

        string damage = "Damage: \t" + unit.baseDamageValue + "/" + unit.damageValue + "\n";
        string health = "Health: \t" + unit.baseHealthValue + "/" + unit.owner.HealthComponent.Health + "\n";
        string defence = "Defence: \t" + unit.baseDefenceValue + "/" + unit.defenceValue + "\n";
        string speed = "Speed: \t" + unit.baseSpeedValue + "/" + unit.speedValue + "\n";
        string initiative = "Initiative: \t" + unit.baseInitiativeValue + "/" + unit.initiativeValue + "\n";
        string range = "Range: \t" + unit.baseRangeValue + "/" + unit.rangeValue;

        endString += health + damage + defence + speed + initiative + range;

        return endString;
    }

    public float Timer(ref float timer) {
        return timer -= Time.deltaTime;
    }
}