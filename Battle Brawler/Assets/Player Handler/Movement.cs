using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Movement : MonoBehaviour {
 
    public float movementSpeed;
    public string chasingWho;
    public Transform getToPosition;
    private bool isJumping = false;
    public string currentAction;
    private Movement enemyState;
    private PlayerCharacteristics playerCharacteristics;
    public Transform groundCheck;
    public bool? BlockAttack;
    bool grounded = false;
    public Transform target;
    private HingeJoint swingArmsHinge;
    public string attackStep;
    private GameObject Hand;
    private Quaternion enemyDirection;
    private float dashTime;

    // Use this for initialization
    void Start () {
        Hand = gameObject.transform.GetChild(1).transform.GetChild(0).gameObject;
        if (transform.name == "Player2")
            chasingWho = "Player1";
        if (transform.name == "Player1") {
            chasingWho = "Player2";
        }
        enemyState = GameObject.Find(chasingWho).GetComponent<Movement>();
        playerCharacteristics = transform.GetComponent<PlayerCharacteristics>();
    }

    void chaseEnemy() {
        currentAction = "Chase enemy";
        target = getToPosition;
        transform.position += transform.TransformDirection (Vector3.forward) * Time.deltaTime * movementSpeed;
    }

    GameObject checkWeapons() {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < weapons.Length; i++ ) {
            if (Vector3.Distance(transform.position, weapons[i].transform.position) < 8 && weapons[i].GetComponent<equipmentCharacteristics>().isEquipped == false)
                return (weapons[i]);
        }
        return (null);
    }

    public void createHinge() {
        swingArmsHinge = Hand.AddComponent<HingeJoint>();
        Hand.GetComponent<Rigidbody>().useGravity = false;
        swingArmsHinge.connectedBody = gameObject.transform.GetComponent<Rigidbody>();
        swingArmsHinge.axis = new Vector3(1, 0, 0);
        swingArmsHinge.anchor = new Vector3(0, 0, -3f);
        JointLimits limits = swingArmsHinge.limits;
        limits.min = -30;
        limits.max = 90;
        swingArmsHinge.limits = limits;
        swingArmsHinge.useLimits = true;
        /*JointSpring hingeSpring = swingArmsHinge.spring;
        hingeSpring.spring = 1;
        hingeSpring.targetPosition = 0;
        swingArmsHinge.spring = hingeSpring;
        swingArmsHinge.useSpring = true;*/
    }

    void goGetWeapon(GameObject weapon) {
        currentAction = "Get weapon";
        if (transform.position.x != weapon.transform.position.x)
            transform.position += transform.TransformDirection (Vector3.forward) * Time.deltaTime * movementSpeed;
        target = weapon.transform;
    }

    bool canTryHitEnemy() {
        float range =  playerCharacteristics.currentMainEquipment.gameObject.GetComponent<Renderer>().bounds.size.z *  playerCharacteristics.currentMainEquipment.gameObject.GetComponent<Renderer>().transform.localScale.z;
        if (playerCharacteristics.isArmed && Vector3.Distance(transform.position, getToPosition.position) < range - 2.5f) {
            return true;
        }
        return false;
    }

    void launchAttack() {
        currentAction = "launch Attack";
        if ((attackStep == "finished" || string.IsNullOrEmpty(attackStep)) && System.Math.Round(swingArmsHinge.velocity, 0) == 0f) {
            //swingArmsHinge.axis = new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0);
            //Debug.Log(swingArmsHinge.axis);
            attackStep = "prepare Hit";
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false;
            var motor = swingArmsHinge.motor;
            motor.force = 100;
            motor.targetVelocity = -100 - Hand.transform.GetChild(0).GetComponent<equipmentCharacteristics>().speed * 99;
            motor.freeSpin = true;
            swingArmsHinge.motor = motor;
            swingArmsHinge.useMotor = true;
        }
        else if (attackStep == "prepare Hit" && System.Math.Round(swingArmsHinge.velocity, 0) == 0f) {
            attackStep = "hit";
            //Debug.Log("Strike");
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Collider>().enabled = true;
            var motor = swingArmsHinge.motor;
            motor.force = 100;
            motor.targetVelocity = 100 + Hand.transform.GetChild(0).GetComponent<equipmentCharacteristics>().speed * 95;
            motor.freeSpin = true;
            swingArmsHinge.motor = motor;
            swingArmsHinge.useMotor = true;
        } else if (attackStep == "hit" && System.Math.Round(swingArmsHinge.velocity, 0) == 0f) {
            //Debug.Log("FINISHED");
            attackStep ="finished";
        }
    }

    bool needToDefend() {
        if (enemyState.attackStep == "hit") {
            return true;
        } else {
            if (BlockAttack != null) {
                Quaternion standardAngle = Quaternion.Euler(transform.GetChild(1).transform.rotation.eulerAngles.x, transform.GetChild(1).transform.rotation.eulerAngles.y, 0);
                transform.GetChild(1).transform.rotation = standardAngle;
            }
            BlockAttack = null;
            return false;
        }
    }

    void defend() {
        if (BlockAttack == true || (BlockAttack != false && Random.Range(0f, 1f) < playerCharacteristics.block && attackStep != "prepare hit" && attackStep != "hit")) {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, enemyDirection.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
                //Vector3 enemyArmAngle = target.GetChild(1).GetChild(0).rotation.eulerAngles;
                Quaternion oppositeAngle = Quaternion.Euler(transform.GetChild(1).transform.rotation.eulerAngles.x, transform.GetChild(1).transform.rotation.eulerAngles.y, -45);
                transform.GetChild(1).transform.rotation = oppositeAngle;
                BlockAttack = true;
                //Debug.Log("DEFEND");
            } else {
                BlockAttack = false;
            }
    }

    bool isOnOneAnother() {
        if ((playerCharacteristics.isArmed && Mathf.Abs(transform.position.x - getToPosition.position.x) < 1) || dashTime != playerCharacteristics.dashDuration) {
            return true;
        }
        return false;
    }

    void dash() {
        Vector3 playerVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        currentAction = "dashing";
        if (dashTime <= 0) {
            dashTime = playerCharacteristics.dashDuration;
            playerVelocity = Vector3.zero;
        } else {
            Debug.Log("DASH");
            dashTime -= Time.deltaTime;
            if (transform.position.x - getToPosition.position.x < 0) {
                playerVelocity = new Vector3(10, 10, 10) * playerCharacteristics.dashSpeed;
            } else {
                playerVelocity = new Vector3(-10, 10, 10) * playerCharacteristics.dashSpeed;
            }
        }
    }
 
    //Update is called once per frame
    void FixedUpdate () {
        getToPosition = GameObject.Find(chasingWho).transform;
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1<< LayerMask.NameToLayer("Ground"));
        /*if (Random.Range(0, 200) == 1) {
            isJumping = true;
        }
        if (isJumping) {
            if (!grounded) {
                transform.position += transform.TransformDirection (Vector3.up) * Time.deltaTime * movementSpeed * 3; 
            } else {
                isJumping = false;
            }
        }*/
        GameObject goEquip = checkWeapons();
        if (goEquip && !playerCharacteristics.isArmed) {
           // Debug.Log(goEquip + " " + playerCharacteristics.isArmed);
            goGetWeapon(goEquip);
        }
        else if (isOnOneAnother()) {
            dash();
        }
        else if (needToDefend()) {
            defend();
        }
        else if (canTryHitEnemy()) {
            launchAttack();
        }
        else {
        chaseEnemy();
        }
        Vector3 relativePos = target.position - transform.position;
        enemyDirection = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion rotationWithoutX = Quaternion.Euler(transform.rotation.eulerAngles.x, enemyDirection.eulerAngles.y, enemyDirection.eulerAngles.z);
        Quaternion rotationOnlyX = Quaternion.Euler(enemyDirection.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        if (BlockAttack != true)
            transform.rotation = rotationWithoutX;
        if (!playerCharacteristics.isArmed)
            transform.GetChild(1).transform.rotation = rotationOnlyX;
        /*if (Input.GetKey ("z")) {
            transform.position += transform.TransformDirection (Vector3.forward) * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey ("s")) {
            transform.position += transform.TransformDirection (Vector3.back) * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey ("q")) {
                transform.position += transform.TransformDirection (Vector3.left) * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey ("d")) {
                transform.position += transform.TransformDirection (Vector3.right) * Time.deltaTime * movementSpeed;
            }
        if (Input.GetKey (KeyCode.Space)) {
            transform.position += transform.TransformDirection (Vector3.up) * Time.deltaTime * movementSpeed;
        }*/
    }
}