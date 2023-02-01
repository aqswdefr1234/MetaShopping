using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListData : MonoBehaviour //ȣ��Ʈ���ƴ� Ŭ���̾�Ʈ ���� ���忡���� ����Ʈ�� ó���� ����ִ� �̷���ȵȴ�.
{
    public List<GameObject> shoppingPlayers = new List<GameObject>();
    public List<GameObject> room1Players = new List<GameObject>();
    public int ownerSceneNumber_;

    void Awake()
    {
        GameObject[] playerListObjects = GameObject.FindGameObjectsWithTag("PlayerListData");
        if (playerListObjects.Length > 1)
            Destroy(gameObject);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
