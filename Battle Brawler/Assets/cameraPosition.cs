using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPosition : MonoBehaviour
{

    public GameObject playerOne;
    public GameObject playerTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerTwo.transform.position.x + (playerOne.transform.position.x - playerTwo.transform.position.x) / 2, transform.position.y, transform.position.z);
    }
}
