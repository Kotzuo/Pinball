using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ball")]
    public GameObject ball;
    public Transform spawnBallPosition;

    [Header("Flippers")]
    public HingeJoint rightPaddle;
    public HingeJoint leftPaddle;
    public float idlePaddleAngle = 0f;
    public float activePaddleAngle = 30f;
    public float paddleSpring = 1000f;
    public float paddleDamper = 2f;

    bool isActive;
    JointSpring idleJointSpring;
    JointSpring activeJointSpring;

    [Header("Launcher")]
    public GameObject launcher;
    public Transform launcherMaxPos;
    public float launcherChargeSpeed = 15;

    Rigidbody launcherRB;
    float launcherAmmout = 0;
    Vector3 launcherInitialPos;

    [Header("Door")]
    public Transform door;
    public BoxCollider doorTrigger;
    public float doorSpeed = 8f;
    public bool isDoorClosed = false;
    public AnimationCurve doorSpeedCurve;

    float doorAmmount = 0;    

    void Start()
    {
        launcherInitialPos = launcher.transform.position;
        launcherRB = launcher.GetComponent<Rigidbody>();

        idleJointSpring = new JointSpring();
        activeJointSpring = new JointSpring();

        idleJointSpring.spring = paddleSpring;
        idleJointSpring.damper = paddleDamper;
        idleJointSpring.targetPosition = idlePaddleAngle;

        activeJointSpring.spring = paddleSpring;
        activeJointSpring.damper = paddleDamper;
        activeJointSpring.targetPosition = activePaddleAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnBall();
            isDoorClosed = false;
        }

        launcherAmmout = Mathf.Clamp(launcherAmmout + (launcherChargeSpeed * (Input.GetKey(KeyCode.LeftControl) ? 1 : -1) * Time.deltaTime), 0, 1);
        doorAmmount = Mathf.Clamp(doorAmmount + (doorSpeed * (isDoorClosed ? 1 : -1) * Time.deltaTime), 0, 1);

        HandlePaddles();
        HandleLauncher();
        HandleDoor();
    }

    void HandlePaddles()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isActive)
            {
                rightPaddle.spring = activeJointSpring;
                activeJointSpring.targetPosition = -activeJointSpring.targetPosition;
                leftPaddle.spring = activeJointSpring;
                activeJointSpring.targetPosition = -activeJointSpring.targetPosition;
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
                rightPaddle.spring = idleJointSpring;
                leftPaddle.spring = idleJointSpring;
                isActive = false;
            }
        }
    }

    void HandleLauncher()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            launcherInitialPos = launcher.transform.position;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!launcherRB.isKinematic)
                launcherRB.isKinematic = true;
            launcher.transform.position = Vector3.Lerp(launcherInitialPos, launcherMaxPos.position, launcherAmmout);
        }
        else
        {
            if (launcherRB.isKinematic)
                launcherRB.isKinematic = false;
        }
    }

    void HandleDoor()
    {
        door.position = new Vector3(door.position.x, doorSpeedCurve.Evaluate(doorAmmount), door.position.z);
    }

    void SpawnBall()
    {
        Instantiate(ball, spawnBallPosition.position, Quaternion.identity);
    }
}
