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
    private Text positionText, populationText, consumptionText;
    public int index = 0;
    private bool changed = false;
    List<Selectable> components = new List<Selectable>();
    private Selectable lastSelected = null;
    float bump = 1.2f;

    Building editable = null;

    List<string> cats = new List<string>()
        {"Default", "Business", "Industrial" };

    public static EditBuilding instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        nameField = gameObject.transform.Find("Name").GetComponent<InputField>();
        nameField.readOnly = true;
        populationText = gameObject.transform.Find("_Text_Population").GetComponent<Text>();
        populationSlider = gameObject.transform.Find("Population").GetComponent<Slider>();
        consumptionText = gameObject.transform.Find("_Text_Consumption").GetComponent<Text>();
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
        GazeSelect.instance.selectedObj = null;
        Hide();
    }

    public void Hide()
    {
        foreach (Renderer r in GameObject.Find("SelectionCube").GetComponentsInChildren<Renderer>())
        {
            r.material.color = new Color(0.886f, 0.75f, 0.414f, 0.75f);
        }

        if (GazeSelect.instance.selectedGameObject != null) Destroy(GazeSelect.instance.selectedGameObject.GetComponent<USizePulser>());
        gameObject.SetActive(false);
        MenuSelect.instance.Show();
    }

    public void Show(Building entity)
    {
        foreach (Renderer r in GameObject.Find("SelectionCube").GetComponentsInChildren<Renderer>())
        {
            r.material.color = new Color(0, 0, 0, 0);
        }

        editable = entity;
        MenuSelect.instance.Hide();
        gameObject.SetActive(true);
        nameField.text = entity.ID;
        populationText.text = "Population: " + entity.Population;
        populationSlider.value = entity.Population;
        consumptionText.text = "Consumption: " + entity.Consumption;
        consumptionSlider.value = entity.Consumption;
        positionText.text = "Position: " + entity.location.x + " x " + entity.location.y;
        catergoryDropDown.value = cats.IndexOf(entity.Category);



        USizePulser usp = GazeSelect.instance.selectedGameObject.AddComponent<USizePulser>();
        usp.setScalingAttributes(new Visualizer.SizePulser(1, 1.5f, 1, 1).floatParams);

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
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (index==1)
            {
                populationText.text = "Population: " + populationSlider.value;
            }
            if (index == 2)
            {
                consumptionText.text = "Consumption: " + consumptionSlider.value;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (index == 1)
            {
                populationText.text = "Population: " + populationSlider.value;
            }
            if (index == 2)
            {
                consumptionText.text = "Consumption: " + consumptionSlider.value;
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            editable.consumption = (int)consumptionSlider.value;
            editable.population = (int)populationSlider.value;
            index = 0;
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
