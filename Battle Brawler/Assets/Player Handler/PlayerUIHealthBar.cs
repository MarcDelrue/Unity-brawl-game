using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider playerHealthSlider;
    public GameObject thePlayer;

    private PlayerCharacteristics playerScript;

    void Start()
    {
        playerHealthSlider = gameObject.GetComponent<Slider>();
        thePlayer = transform.parent.parent.gameObject;
        playerScript = thePlayer.GetComponent<PlayerCharacteristics>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = thePlayer.transform.position + new Vector3(0, 2f, 0);
        float test = playerScript.currentLife / playerScript.maxLife;
        playerHealthSlider.value = test;
    }
}
