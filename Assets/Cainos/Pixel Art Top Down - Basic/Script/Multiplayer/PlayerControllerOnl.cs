using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class PlayerControllerOnl : MonoBehaviour
{

    //[SerializeField] private Transform camTransform;
    private Animator animator;
    private bool[] inputs;
    [SerializeField] Transform firePoint ;
    bool active = false;
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        inputs = new bool[4];
    }
  
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftControl))
        {
            if (active == false)
                active = true;
            else
                active = false;
        }

        //last = firePoint.position;
        //Debug.Log("last : "+last);
        if (Input.GetKey(KeyCode.W) && active == false)
        {
            inputs[0] = true;
            animator.SetInteger("Direction", 1);
        }
        if (Input.GetKey(KeyCode.S) && active == false)
        {
            inputs[1] = true;
            animator.SetInteger("Direction", 0);
        }
        if (Input.GetKey(KeyCode.A) && active == false)
        {
            inputs[2] = true;
            animator.SetInteger("Direction", 3);
        }
        if (Input.GetKey(KeyCode.D) && active == false)
        {
            inputs[3] = true;
            animator.SetInteger("Direction", 2);
        }
        
        animator.SetBool("IsMoving", 1 > 0);

        if (Input.GetKeyDown(KeyCode.Space) && active == false)
        {
            SendShootToServer();

        }
        RotateFirePoint();
    }
    void SendShootToServer()
    {
        // Tạo thông điệp gửi đến server
        Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ClientToServerId.shoot);
        // Gửi thông điệp đến server
        NetworkManager.Singleton.Client.Send(message);
    }

    void RotateFirePoint()
    {
        float angle = Utility.AngleTowardsMouse(firePoint.position);
        firePoint.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //abc.position
        //Debug.Log(firePoint.position);
    }
    private void FixedUpdate()
    {


        SendInput();

        for (int i = 0; i < inputs.Length; i++)
            inputs[i] = false;
    }

    #region Messages
    private void SendInput()
    {
       
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.input);
        message.AddBools(inputs, false);
        //Debug.Log("gui :" + firePoint.rotation);
        message.AddQuaternion(firePoint.rotation);
        NetworkManager.Singleton.Client.Send(message);
    }
    #endregion
}
