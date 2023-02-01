using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManagerActivation : MonoBehaviour
{
    public GameObject NetworkManagerObject;
    public GameObject BtnPanel;
    void Awake()
    {
        GameObject[] netManagerObject = GameObject.FindGameObjectsWithTag("NetworkManager");//��Ȱ��ȭ �� ������Ʈ�� ã�� �� ����. ��Ʈ��ũ �Ŵ��� ������Ʈ�� ������ �� ��Ȱ��ȭ ���·� �����ϱ� ������ �˻���
                                                                                            //���ӿ�����Ʈ�� 0���̴�.
        if (netManagerObject.Length > 0)
        {
            Debug.Log("��Ʈ��ũ �Ŵ����� �̹� �����մϴ�.");
            BtnPanel.SetActive(false);
        }
        else
            NetworkManagerObject.SetActive(true);
    }
}
