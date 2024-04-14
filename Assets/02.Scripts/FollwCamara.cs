using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwCamara : MonoBehaviour
{
    // ���󰡾��� ���
    public Transform targetTr;
    // Main Camera �ڽ��� Transform ������Ʈ 
    private Transform camTr;

    // ���� ������κ��� ������ �Ÿ�
    [Range(0.0f, 10.0f)]
    public float distance = 10.0f;

    // Y������ �̵��� ����
    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    void Start()
    {
        // Main Camera �ڽ��� Transform ������Ʈ�� ����
        camTr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // �����ؾ� �� ����� �������� distance��ŭ �̵�
        // ���̸� height��ŭ �̵�
        camTr.position = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);

        // Camera�� �ǹ� ��ǥ�� ���� ȸ��
        camTr.LookAt(targetTr.position);
    }
}
