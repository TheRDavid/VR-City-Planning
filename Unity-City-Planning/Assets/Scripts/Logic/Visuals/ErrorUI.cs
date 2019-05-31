using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorUI : MonoBehaviour
{
    private static int windowWidth = 360, windowHeight = 240;
    private static int windowX = Screen.width / 2 - windowWidth / 2, windowY = Screen.height / 2 - windowHeight / 2;
    private static int windowPaddingX = 10, windowPaddingY = 20;
    private static int windowButtonWidth = 120, windowButtonHeight = 30;
    private Rect windowRect = new Rect(windowX, windowY, windowWidth, windowHeight);
    private Stack<string> messages = new Stack<string>();
    private string currentMessage = "";

    public void displayMessage(string msg)
    {
        messages.Push(msg);
    }

    private void OnGUI()
    {
        if (currentMessage.Length == 0 && messages.Count > 0)
        {
            currentMessage = messages.Pop();
        }

        if (currentMessage.Length > 0) GUI.Window(0, windowRect, drawErrorWindow, "ERROR");

    }

    void drawErrorWindow(int windowID)
    {
        GUI.Label(new Rect(windowPaddingX, windowPaddingY, windowWidth - windowPaddingX * 2, windowHeight - windowPaddingY * 2), currentMessage);

        if (GUI.Button(new Rect(windowWidth / 2 - windowButtonWidth / 2, windowHeight - windowPaddingY - windowButtonHeight, windowButtonWidth, windowButtonHeight), "Darn it"))
        {
            currentMessage = "";
        }
    }
}
