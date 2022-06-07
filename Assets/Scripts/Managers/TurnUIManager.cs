using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUIManager
{
    public TurnUIManager (TurnManager Owner, List<GameObject> buttons, GameObject Text, GameObject EndScreen) {
        for (int i = 0; i < buttons.Count; i++) {
            if (i < 2)
                FunctionButtons.Add(buttons[i]);
            else
                AbilityButtons.Add(buttons[i]);
        }

        this.Owner = Owner;
        this.EndScreen = EndScreen;
        TurnText = Text.GetComponent<Text>();
    }

    public TurnManager Owner;

    public List<GameObject> FunctionButtons = new List<GameObject>();
    public List<GameObject> AbilityButtons = new List<GameObject>();
    public Text TurnText;
    public GameObject EndScreen;

    public void ActivateButtons() {
        foreach (var button in FunctionButtons) {
            button.GetComponent<Button>().interactable = true;
            button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        foreach (var button in AbilityButtons) {
            button.GetComponent<Button>().interactable = true;
        }
    }
    public void DeactivateButtons() {
        foreach (var button in FunctionButtons) {
            button.GetComponent<Button>().interactable = false;
            button.transform.GetChild(0).GetComponent<Image>().color = new Color(.5f, .5f, .5f, 1);
        }
        foreach (var button in AbilityButtons) {
            var buttonClickable = button.GetComponent<Button>();
            buttonClickable.interactable = false;
            buttonClickable.onClick.RemoveAllListeners();
            button.GetComponent<ToolTipTrigger>().TextToShow = "";
            button.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetAbilities(List<AbilityBase> abilities, UnitManager AbilityOwner) {
        if (abilities.Count > 3) {
            Debug.LogError("Ability Count Higher than 3, please Remove " + (abilities.Count - 3) + " Abilities!");
            return;
        }

        int ButtonsUsed = 0;

        for (int i = 0; i < abilities.Count; i++) {
            var buttonGO = AbilityButtons[i];
            buttonGO.GetComponent<ToolTipTrigger>().TextToShow = abilities[i].Description;

            buttonGO.transform.GetChild(0).gameObject.SetActive(true);
            var image = buttonGO.transform.GetChild(0).GetComponent<Image>();
            image.sprite = abilities[i].Icon;

            var funcButton = buttonGO.GetComponent<Button>();
            funcButton.onClick.RemoveAllListeners();

            if (abilities[i].currentCooldown < 1) {
                image.color = new Color(1f, 1f, 1f, 1);
                funcButton.interactable = true;
                var param = i;
                funcButton.onClick.AddListener(delegate{AbilityOwner.SelectAbility(param);});
            }
            else {
                image.color = new Color(.5f, .5f, .5f, 1); 
                funcButton.interactable = false;
            }

            ButtonsUsed++;
        }

        for (int i = ButtonsUsed; i < 3; i++) {
            AbilityButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            AbilityButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    public void SetInfoText(string newText) {
        TurnText.text = newText;
    }

    public void ShowEndScreen(string Header, string Body) {
        EndScreen.SetActive(true);
        var texts = EndScreen.GetComponentsInChildren<Text>();
        texts[0].text = Header;
        texts[1].text = Body;

        DeactivateButtons();

        var funcbutton = EndScreen.transform.GetChild(0).GetComponent<Button>();
        funcbutton.onClick.AddListener(delegate { CloseEndScreen(); });
    }
    public void CloseEndScreen() {
        Owner.isDone = true;
    }
}