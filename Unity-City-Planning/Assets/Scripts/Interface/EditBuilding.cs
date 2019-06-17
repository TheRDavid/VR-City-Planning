using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditBuilding : MonoBehaviour
{
    private InputField nameField;
    private Slider populationSlider, consumptionSlider;
    private Dropdown catergoryDropDown;
    private Button deleteButton;
    private Button duplicateButton;
    private Text positionText;
    public int index = 0;
    private bool changed = false;
    List<Selectable> components = new List<Selectable>();
    private Selectable lastSelected = null;
    float bump = 1.2f;

    List<string> cats = new List<string>()
        {"Default", "Business", "Industrial" };

    public static EditBuilding instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        nameField = gameObject.transform.Find("Name").GetComponent<InputField>();
        populationSlider = gameObject.transform.Find("Population").GetComponent<Slider>();
        consumptionSlider = gameObject.transform.Find("Consumption").GetComponent<Slider>();
        positionText = gameObject.transform.Find("Position").GetComponent<Text>();
        catergoryDropDown = gameObject.transform.Find("Category").GetComponent<Dropdown>();
        deleteButton = gameObject.transform.Find("Delete").GetComponent<Button>();
        duplicateButton = gameObject.transform.Find("Duplicate").GetComponent<Button>();

        components.Add(nameField);
        components.Add(populationSlider);
        components.Add(consumptionSlider);
        components.Add(catergoryDropDown);
        components.Add(duplicateButton);
        components.Add(deleteButton);

        deleteButton.onClick.AddListener(DeleteClicked);
        gameObject.SetActive(false);
    }

    private void DeleteClicked()
    {
        Municipality.deleteMapEntity(GazeSelect.instance.selectedObj);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        MenuSelect.instance.Show();
    }

    public void Show(Building entity)
    {
        MenuSelect.instance.Hide();
        gameObject.SetActive(true);
        nameField.text = entity.ID;
        populationSlider.value = entity.Population;
        consumptionSlider.value = entity.Consumption;
        positionText.text = "Position: " + entity.location.x + " x " + entity.location.y;
        catergoryDropDown.value = cats.IndexOf(entity.Category);
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            changed = true;
            index++;
            if (index >= components.Count) index = 0;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            changed = true;
            index--;
            if (index < 0) index = components.Count - 1;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Hide();
        }

        try
        {
            if (changed)
            {
                changed = false;
                if (lastSelected != null)
                {
                    lastSelected.transform.localScale /= bump;
                }
                lastSelected = components[index];
                lastSelected.Select();
                lastSelected.transform.localScale *= bump;
            }
        }
        catch
        {
        }
    }
}
