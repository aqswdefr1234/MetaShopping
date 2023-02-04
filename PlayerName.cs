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
            {
                forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ = 0;
            }
            else if (SceneManager.GetActiveScene().name == "Room1")
            {
                forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ = 1;
            }
                
            networkPlayersNames.Value = GameObject.Find("NetManager").GetComponent<RelayServerManager>().onNetworkSpawn_PlayerName;
            networkPlayersSceneNumber.Value = forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_;
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
        if (!IsOwner)//���� ���۽� �ڷ�ƾ���� ���� ���� ���� �÷��̾� ��Ȱ��ȭ �� ����Ʈ �߰��� �����ϹǷ� previous != -1 ������ �߰��Ѵ�
        {           //�׷��� �ؾ� ���� ����� ���� ���� �������⵵ ���� ��Ȱ��ȭ ��Ű�°� ���� �� �ִ�.
            if (previous != -1 && forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ == current)
            {
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                gameObject.SetActive(true);
            }
            else if(previous != -1 && forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ != current)
            {
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                gameObject.SetActive(false);
            }
            if(previous != -1 && current == 0) //���ε��� �÷��̾ WaitLoadingName �ڷ�ƾ���� ����Ʈ�� �߰��ǰ�, ���⼭�� �߰��Ǹ� �ȵǹǷ� previous != -1 ���� �־���
            {
                Debug.Log("current : " + current);
                forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Add(gameObject);
                forOwnerDataLoad.GetComponent<PlayerListData>().room1Players.Remove(gameObject);
            }
            else if (previous != -1 && current == 1)
            {
                Debug.Log("current : " + current);
                forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Remove(gameObject);
                forOwnerDataLoad.GetComponent<PlayerListData>().room1Players.Add(gameObject);
            }
        }
        else
        {
            if(previous != -1 && current == 0)
            {
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers, true);
                PlayerListIsVisible(forOwnerDataLoad.GetComponent<PlayerListData>().room1Players, false);
            }
            else if (previous != -1 && current == 1)
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
            if (forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ == -1 || networkPlayersNames.Value == "" || networkPlayersSceneNumber.Value == -1)
                yield return new WaitForSeconds(0.2f);
            else
            {
                playerName3D.text = networkPlayersNames.Value.ToString();
                gameObject.name = playerName3D.text;// ������ �÷��̾��� �̸� �ٲ�. ������ �ٲ�µ� �ð��� �ɸ���. �׷��� ������Ʈ�� ��Ȱ��ȭ ��ų�� ������Ʈ �̸��� �ٲ������ Ȯ�� �� ��Ȱ��ȭ ���Ѿ��Ѵ�.
                if (networkPlayersSceneNumber.Value == 0)
                    forOwnerDataLoad.GetComponent<PlayerListData>().shoppingPlayers.Add(gameObject);
                else if (networkPlayersSceneNumber.Value == 1)
                    forOwnerDataLoad.GetComponent<PlayerListData>().room1Players.Add(gameObject);
                if (forOwnerDataLoad.GetComponent<PlayerListData>().ownerSceneNumber_ != networkPlayersSceneNumber.Value)//������ ������� �����÷��̾���� ��ġ�� ���ٸ� Ȱ��ȭ �ƴϸ� ��Ȱ��ȭ
                {//OnSomeValueChanged ������ ��Ż�� ���� ���� �̵��� ��(+���ο� ������ ���� ��)�� �۵��Ǳ⶧���� ������ �־��� �÷��̾���� �ε��� �� Ȱ��ȭ/��Ȱ��ȭ �۾��� �ʿ��ϴ�.
                    Debug.Log(gameObject.name + " : false");
                    gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    gameObject.SetActive(false);
                }
                break;
            }
        }
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
