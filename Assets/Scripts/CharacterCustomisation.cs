using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomisation : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float rotationFactor = 10f;
    private Camera mainCamera;
    private Vector3 mousePosition;
    private CharacterJoint joint;
    private List<Vector3[]> originalJointInfo = new List<Vector3[]>();
    private float currentXRotation = 0;
    private float currentYRotation = 0;
    private bool movingJoint = false;
    private float currentJointRotation = 0;

    void Start()
    {
        mainCamera = Camera.main;
        foreach (CharacterJoint joint in target.GetComponentsInChildren<CharacterJoint>())
        {
            originalJointInfo.Add(new Vector3[2] {joint.transform.position,joint.transform.rotation.eulerAngles});
        }

    }

    void Update()
    {
        ClickDetection();
        MoveJoint();
    }

    public void ChangeValue(string valueName, float value)
    {
        //Check if rotation slider is being used
        if (valueName.Contains("Rotation"))
        {
            //Perform worldspace x rotation
            if (valueName.Contains("X"))
            {
                target.transform.Rotate(new Vector3(value - currentXRotation, 0, 0), Space.World);
                currentXRotation = value;
            }
            //Perform worldspace y rotation
            if (valueName.Contains("Y"))
            {
                target.transform.Rotate(new Vector3(0, value - currentYRotation, 0), Space.World);
                currentYRotation = value;
            }
        }
    }

    public void ActivateFunction(string functionName)
    {
        if (functionName.Contains("ResetRotation"))
        {
            //Reset rotation
            target.transform.rotation = Quaternion.identity;

            //Reset Sliders
            currentXRotation = 0;
            currentYRotation = 0;
            Slider[] sliders = GetComponentsInChildren<Slider>();

            foreach (Slider slider in sliders)
            {
                if (slider.name.Contains("Rotation"))
                    slider.value = 0;
            }
        }

        if (functionName.Contains("ResetPose"))
        {
            CharacterJoint[] joints = target.GetComponentsInChildren<CharacterJoint>();


            for (int i = 0; i < joints.Length; i++)
            {
                joints[i].transform.rotation = Quaternion.Euler(originalJointInfo[i][1]);
                joints[i].transform.position = originalJointInfo[i][0];
            }
        }
    }

    private void ClickDetection()
    {
        //Check if mouse has been pressed down
        if (Input.GetMouseButtonDown(0))
        {
            //Activate rotation
            movingJoint = true;

            //mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Cast ray from mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Raycast hit");
                if (hit.collider.GetComponent<CharacterJoint>() != null)
                {
                    joint = hit.collider.GetComponent<CharacterJoint>();
                    //Initialise current joint rotation
                    mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 rotation = mousePosition - joint.transform.position;
                    currentJointRotation = Mathf.Atan2(rotation.y, rotation.z) * Mathf.Rad2Deg * rotationFactor;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Deactivate rotation and reset joint variables
            movingJoint = false;
            joint = null;
            currentJointRotation = 0;
        }
    }

    private void MoveJoint()
    {
        if (movingJoint && joint != null)
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePosition - joint.transform.position;
            float zRotation = Mathf.Atan2(rotation.y, rotation.z) * Mathf.Rad2Deg * rotationFactor; // <--TEMP FIX, remove later
            Debug.Log("Mouse Rotation: " + zRotation + " | Current Rotation: " + currentJointRotation);
            float zAmountToRotate = currentJointRotation - zRotation;
            //Assign new current rotation
            currentJointRotation = zRotation;

            joint.transform.Rotate(new Vector3(0, 0, zAmountToRotate), Space.World);
            //joint.transform.Rotate(new Vector3(0, 0, zRotation), Space.World);
        }
    }
}
