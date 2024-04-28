using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : MonoBehaviour
{
    // ���Ͱ� ������ ��ġ�� ������ �迭
    public List<Transform> points = new List<Transform>();

    // ���� �������� ������ ����
    public GameObject monster;

    // ������ ���� ����
    public float createTime = 3.0f;

    // ������ ���� ���θ� ������ ��� ����
    private bool isGameOver;

    // ������ ���� ���θ� ������ ������Ƽ
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    void Start()
    {
        // SpawnPointGroup ���� ������Ʈ�� Transform ������Ʈ ����
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        // SpawnPointGroup ������ �ִ� ��� ���ϵ� ���ӿ�����Ʈ�� Transform ������Ʈ ����
        foreach(Transform point in transform)
        {
            points.Add(point);
        }

        // ������ �ð� �������� �Լ��� ȣ��
        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }
    void CreateMonster()
    {
        // ������ �ұ�Ģ�� ���� ��ġ ����
        int idx = Random.Range(0, points.Count);
        // ���� ������ ����
        Instantiate(monster, points[idx].position, points[idx].rotation);
    }
}
