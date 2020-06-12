using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    public string playerName;
    public float currentLife;
    public float maxLife;
    public float block;
    public bool isArmed;
    public float dashSpeed;
    public float dashDuration;
    public GameObject currentMainEquipment;
    public GameObject currentSecondaryEquipment;
    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
