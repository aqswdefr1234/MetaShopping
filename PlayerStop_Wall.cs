using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStop_Wall : MonoBehaviour
{
    private int trigger = 0;
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("�ݸ���enter �۵���");
        if(trigger == 0)
        {
            if (other.collider.CompareTag("Player")) //Ʈ���ſ����� other.tga == �� �����ϰ� �±� ��밡��
            {
                other.transform.parent.GetComponent<PlayerMoveAndCamera>().speedSetting = 1f;
                trigger = 1;
            }
        }
    }
    void OnCollisionExit(Collision other)
    {
        Debug.Log("�ݸ���exit �۵���");
        if (trigger == 1)
        {
            if (other.collider.CompareTag("Player"))
            {
                other.transform.parent.GetComponent<PlayerMoveAndCamera>().speedSetting = 10f;
                trigger = 0;
            }
        }
    }
}
