using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // �ʱ� ���� ��
    private readonly float initHp = 100.0f;
    // ���� ���� ��
    public float curHp;
    // Hpbar ������ ����
    private Image hpBar;

    // ��������Ʈ ����
    public delegate void PlayerDieHandle();
    // �̺�Ʈ ����
    public static event PlayerDieHandle OnPlayerDie;

    IEnumerator Start()
    {
        // Hpbar ����
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        // Hp �ʱ�ȭ
        curHp = initHp;
        DisplayHealth();

        // ������Ʈ�� ������ ������ ����
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        //�ִϸ��̼� ����
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;
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
    private void OnTriggerEnter(Collider collision)
    {
        // �浹�� Collider�� ������ PUNCH�̸� Player�� HP ����
        if (curHp >= 0.0f && collision.CompareTag("PUNCH"))
        {
            curHp -= 10.0f;
            DisplayHealth();

            Debug.Log($"Player hp = {curHp / initHp}");

            // Player�� ������ 0 �����̸� ��� ó��
            if (curHp < 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // Player�� ���ó��
    void PlayerDie()
    {
        Debug.Log("Player Die !");

        // ���ΰ� ��� �̺�Ʈ ȣ��(�߻�)
        OnPlayerDie();

        // GameManager ��ũ��Ʈ�� IsGameOver ������Ƽ ���� ����
        //GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
        GameManager.instance.IsGameOver = true;
    }
    void DisplayHealth()
    {
        hpBar.fillAmount = curHp / initHp;
    }
}
