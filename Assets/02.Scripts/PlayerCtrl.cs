using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // ������Ʈ�� ĳ�� ó���� ����
    [SerializeField] private Transform tr;
    // Animation ������Ʈ�� ������ ����
    [SerializeField] private Animation anim;

    // �̵� �ӷ� ���� (public���� ����Ǿ� �ν����� �信 �����)
    public float movespeed = 10f;
    // ȸ�� �ӵ� ����
    public float turnSpeed = 80.00f;

    void Start()
    {
        // ������Ʈ�� ������ ������ ����
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        //�ִϸ��̼� ����
        anim.Play("Idle");
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(�̵� ���� + �ӷ� * Time.deltaTime)
        tr.Translate(moveDir.normalized * movespeed * Time.deltaTime);

        // Vector3.up ���� �������� turnSpeed��ŭ�� �ӵ��� ȸ��
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // ���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        //Ű���� �Է°��� �������� ������ �ִϸ��̼� ����

        if(v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f); // ���� �ִϸ��̼� ����
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f); // ���� �ִϸ��̼� ����
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f); // ������ �ִϸ��̼� ����
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f); // ���� �ִϸ��̼� ����
        }
        else
        {
            anim.CrossFade("Idle", 0.25f); // ���� �� Idle �ִϸ��̼� ����
        }
    }
}
