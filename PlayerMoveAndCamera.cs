using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMoveAndCamera : NetworkBehaviour 
{
    private GameObject followCamera;
    public float speedSetting;
    public float turnSpeed;
    private Rigidbody myRigid;
    private float xRotate;
    private float yRotateSize;
    private float yRotate;
    private float xRotateSize;
    public GameObject myLocalObject;
    void Start()
    {
        followCamera = GameObject.Find("FollowCamera");
        myRigid = myLocalObject.GetComponent<Rigidbody>();
        DontDestroyOnLoad(followCamera);
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (!IsOwner) return;
        FollowCamera();
        PlayerMove_keybord();
        Look();
        //�̰��� �÷��̾� ������Ʈ ���� �پ������Ƿ� ���� ������� �� ī�޶� �ش� �÷��̾ ���� �´ٰ� �ص� , �ٸ� ������ ������ �����÷��̾��� �������� �ٸ������� �پ��ִ� ��ũ��Ʈ�� �۵��ȴ�.
        //�� ������ ������ ī�޶� �ϳ��� �ִٰ� �ص� �����÷��̾� ���忡���� 2������ ��޵Ǳ⶧����!IsOwner�� �ʿ��ϴ�.
    }
    void FollowCamera()
    {
        followCamera.transform.position = new Vector3(myLocalObject.transform.position.x, myLocalObject.transform.position.y + 0.5f, myLocalObject.transform.position.z);
    }
    void PlayerMove_keybord()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        Vector3 moveVec = (hAxis * myLocalObject.transform.right + vAxis * myLocalObject.transform.forward).normalized;//transform.right �� �ٶ󺸴� ���� ���� ������                                                                                   //transform.forward�� �ٶ󺸴� ���� ���� ����
        myLocalObject.transform.position = (myLocalObject.transform.position + moveVec * speedSetting * Time.deltaTime);//https://docs.unity3d.com/ScriptReference/Rigidbody.MovePosition.html
    }
    private void Look()//https://itadventure.tistory.com/390
    {
        yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        yRotate = myLocalObject.transform.eulerAngles.y + yRotateSize;
        xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed; //�÷��̾ �ϴ��� �ٶ󺸷��� Rotation x�� ������ �Ǿ�� �ϹǷ� Input.GetAxis("Mouse Y") ���� ������ �ٲ��ش�.
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        followCamera.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        myLocalObject.transform.eulerAngles = new Vector3(0, yRotate, 0); //�ٶ󺸴� ������ y���� �޾ƿ� �÷��̾ �ٶ󺸴� ����� �Ȱ��� ȸ�������ش�.
    }
}
