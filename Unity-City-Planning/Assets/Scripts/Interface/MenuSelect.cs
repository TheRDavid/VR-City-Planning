using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour
{
    public static MenuSelect instance;
    List<Button> buttons = new List<Button>();
    Button lastButton = null;
    public int index = 0;
    // index mapping:
    // 0 = eye
    // 1 = default
    // 2 = business
    // 3 = factory
    // 4 = road

    float bump = 1.2f;
    bool changed = false;
    void Start()
    {
        instance = this;
        foreach(Transform t in transform)
        {
            if(!t.name.StartsWith("_"))
            {
                buttons.Add(t.gameObject.GetComponent<Button>());
            }
        }
        buttons[index].Select();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            changed = true;
            index++;
            if (index >= buttons.Count) index = 0;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            changed = true;
            index--;
            if (index < 0) index = buttons.Count - 1;
        }

        try
        {
            if (changed)
            {
                changed = false;
                if (lastButton != null)
                {
                    lastButton.transform.localScale /= bump;
                }
                lastButton = buttons[index];
                lastButton.Select();
                lastButton.transform.localScale *= bump;
            }
        }
            catch
        {
        }
        Debug.Log(index);
    }
}
