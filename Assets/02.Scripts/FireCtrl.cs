using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject bullet;

    //�Ѿ� �߻� ��ǥ
    public Transform firePos;

    private void Start()
    {
        
    }

    private void Update()
    {
        //���콺 ���� ��ư�� Ŭ������ �� Fire �Լ� ȣ��
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Bullet �������� �������� ����(������ ��ü, ��ġ, ȸ��)
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
