using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameStatus : MonoBehaviour
{
    private Text text;

    void createPlayerNameText(GameObject canvas, string GOname, TextAnchor position) {
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        // Create the Text GameObject.
        GameObject playerName = new GameObject();
        playerName.name = GOname;
        playerName.transform.parent = canvas.transform;
        playerName.AddComponent<Text>();

        // Set Text component properties.
        text = playerName.GetComponent<Text>();
        text.font = arial;
        text.text = GameObject.Find("/"+GOname).GetComponent<PlayerCharacteristics>().playerName;
        text.fontSize = 28;
        text.alignment = position;

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform;
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(600, 200);
    }

    void Awake()
    {
        // Create Canvas GameObject.
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        createPlayerNameText(canvasGO, "Player1", TextAnchor.UpperLeft);
        createPlayerNameText(canvasGO, "Player2", TextAnchor.UpperRight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
