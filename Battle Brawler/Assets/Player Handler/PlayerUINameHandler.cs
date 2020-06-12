using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUINameHandler : MonoBehaviour
{
    public Text Name_UIText;
    public GameObject thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = transform.parent.parent.gameObject;
        PlayerCharacteristics playerScript = thePlayer.GetComponent<PlayerCharacteristics>();
        Name_UIText.text = playerScript.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        Name_UIText.transform.position = thePlayer.transform.position + new Vector3(0, 3, 0);
    }
}
