using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipmentDefaultInterraction : MonoBehaviour
{
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack() {
        //GetComponent<Rigidbody>().AddTorque(transform.right * gameObject.GetComponent<equipmentCharacteristics>().speed);
    }

    public void equip(string animationName, Collider other) {
        if (anim && anim.isPlaying)
        {
            return;
        }
        GameObject player = other.gameObject.transform.root.gameObject;
        var playerCharacteristics = player.gameObject.GetComponent<PlayerCharacteristics>();
        if (string.Compare(player.gameObject.tag, "Player") == 0 && !gameObject.GetComponent<equipmentCharacteristics>().isEquipped) {
            playerCharacteristics.isArmed = true;
            if (!playerCharacteristics.currentMainEquipment) {
                playerCharacteristics.currentMainEquipment = gameObject;
            } else if (!playerCharacteristics.currentSecondaryEquipment) {
                 playerCharacteristics.currentSecondaryEquipment = gameObject;
            }
            gameObject.GetComponent<equipmentCharacteristics>().isEquipped = true;
            gameObject.GetComponent<equipmentCharacteristics>().carrier = player.gameObject;
            gameObject.transform.parent = gameObject.GetComponent<equipmentCharacteristics>().carrier.transform.GetChild(1).transform.Find("Hand").transform;
            gameObject.transform.localPosition = Vector3.zero;
            Quaternion levelArms = player.transform.GetChild(1).rotation;
            levelArms = Quaternion.Euler(0, levelArms.eulerAngles.y, levelArms.eulerAngles.z);
            player.transform.GetChild(1).transform.rotation= levelArms;
            player.gameObject.GetComponent<Movement>().createHinge();
            //gameObject.GetComponent<Collider>().isTrigger = false;
            anim.Play(animationName);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float dmg = 0;
        if (collision.collider.transform.root != transform.root) {
            dmg = (collision.relativeVelocity.magnitude * 0.1f * transform.GetComponent<equipmentCharacteristics>().damage);
            //Debug.Log(transform.root.name + " hitted " + collision.collider.name + " inflicting :" + dmg);
            collision.collider.transform.root.GetComponent<PlayerCharacteristics>().currentLife -= dmg;
            if (collision.collider.transform.root.GetComponent<PlayerCharacteristics>().currentLife < 0)
            Destroy(collision.collider.transform.root.gameObject);
        }
    }
}
