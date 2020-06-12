using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Animation anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other) {
        if (!transform.GetComponent<equipmentCharacteristics>().isEquipped)
        transform.GetComponent<equipmentDefaultInterraction>().equip("pickUpLongSword", other);
    }
}
