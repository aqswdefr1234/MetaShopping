using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
using UnityEngine.SceneManagement;
public class PlayerName : NetworkBehaviour//https://docs-multiplayer.unity3d.com/netcode/1.0.0/basics/networkvariable/index.html
{
    [SerializeField] private TMP_Text playerName3D;
    [SerializeField] private NetworkVariable<FixedString128Bytes> networkPlayersNames = new NetworkVariable<FixedString128Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private GameObject playerColliderObject;
    public NetworkVariable<int> networkPlayersSceneNumber = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //networkVariable�� static���� �����ߴ��� ������ �� ���� ������ ���� ���� ��� �����ִ�. ���Ŀ� static�� ���ִ��� �� ������ ����Ǿ��ִ� ���� �׷��� ������ ��찡 ���ܹ����Ƿ� static�� ����Ҷ��� ����!
    public int scene_Number = -1; //������ ���� �� ��
    public int ownerPlayerClientId = -1; //���ʰ� �����ɶ� ���̵� ���� �־���. ���ʸ� ������ Ŭ���̾�Ʈ�� -1
    private GameObject forOwnerDataLoad;
    void Awake()
    {
        forOwnerDataLoad = GameObject.Find("PlayerListData");
    }
    public override void OnNetworkSpawn()//���� �ִ� ��ü������ ���ư�.
    {
        if (IsOwner)
        {
            if(SceneManager.GetActiveScene().name == "Shopping")
                scene_Number = 0;
            else if (SceneManager.GetActiveScene().name == "Room1")
                scene_Number = 1;
            
            networkPlayersNames.Value = GameObject.Find("Canvas").transform.Find("Visible_Nickname").GetComponent<TMP_Text>().text;
            networkPlayersSceneNumber.Value = scene_Number;
            playerName3D.text = networkPlayersNames.Value.ToString();
            gameObject.name = networkPlayersNames.Value.ToString();// ������ �÷��̾��� �̸� �ٲ�.
            playerColliderObject.name = gameObject.name;
            ownerPlayerClientId = Convert.ToInt32(OwnerClientId);
        }
        else
        {
            StartCoroutine(WaitLoadingName());
        }
        networkPlayersSceneNumber.OnValueChanged += OnSomeValueChanged; //�� ��ȭ�� �̺�Ʈ �߻�
    }
    private void OnSomeValueChanged(int previous, int current) //�ش������Ʈ�� ���ΰ� ������.������ ���� �������� ���ؼ��� �ܺο� �ִ� �����͸� �������� ������� �ؾ���
    {
        Debug.Log($"{gameObject.name}������: {previous} | �̵��� ���� ��: {current}");
        if (!IsOwner)
        {
            if (forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ == current)
            {
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                gameObject.SetActive(false);
            }
            if(current == 0)
            {
                forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Add(gameObject);
                forOwnerDataLoad.GetComponent<PlayerListData>().room1Players.Remove(gameObject);
            }
            else if (current == 1)
            {
                forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Remove(gameObject);
                forOwnerDataLoad.GetComponent<PlayerListData>().room1Players.Add(gameObject);
            }
        }
        else
        {
            if(current == 0)
            {
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers, true);
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().room1Players, false);
            }
            else if (current == 1)
            {
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers, false);
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().room1Players, true);
            }
        }
    }
    IEnumerator WaitLoadingName()//NetworkVariable(T value = default, NetworkVariableReadPermission readPerm = NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission writePerm = NetworkVariableWritePermission.Server);
    {
        while (true)
        {
            Debug.Log("WaitLoadingName");
            if (networkPlayersNames.Value == "")
                yield return new WaitForSeconds(0.2f);
            else
                break;
            
            Debug.Log(networkPlayersNames.Value);
        }
        playerName3D.text = networkPlayersNames.Value.ToString();
        gameObject.name = playerName3D.text;// ������ �÷��̾��� �̸� �ٲ�.
        forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Add(gameObject);
    }
    void PlayerListIsVisible(List<GameObject> list,bool isTrue)
    {
        foreach (GameObject obj in list)
        {
            obj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = isTrue;
            obj.SetActive(isTrue);
        }
    }
}
