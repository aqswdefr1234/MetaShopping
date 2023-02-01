using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class IsPlayersVisible : MonoBehaviour
{
    public int ownerSceneNumber = -1;//PlayerName ��ũ��Ʈ���� ���ʰ� �����Ǹ� �� �ѹ��� �־���.
    private GameObject dataObject;
    
    void Start()
    {
        dataObject = GameObject.Find("PlayerListData");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<PlayerName>().ownerPlayerClientId != -1)
        {
            
            if (gameObject.tag == "Shopping_Portal") // �� ������Ʈ�� �� 1�� ����
            {
                dataObject.GetComponent<PlayerListData>().ownerSceneNumber_ = 0;
                other.transform.parent.GetComponent<PlayerName>().scene_Number = 0;
                other.transform.parent.GetComponent<PlayerName>().networkPlayersSceneNumber.Value = 0;
                SceneManager.LoadScene("Shopping");
                other.transform.position = new Vector3(0f,5f,0f);
            }
            else if (gameObject.tag == "Room1_Portal")
            {
                dataObject.GetComponent<PlayerListData>().ownerSceneNumber_ = 1;
                other.transform.parent.GetComponent<PlayerName>().scene_Number = 0;
                other.transform.parent.GetComponent<PlayerName>().networkPlayersSceneNumber.Value = 1;
                SceneManager.LoadScene("Room1");
                other.transform.position = new Vector3(0f, 0f, 0f);
            }
        }
    }
}
