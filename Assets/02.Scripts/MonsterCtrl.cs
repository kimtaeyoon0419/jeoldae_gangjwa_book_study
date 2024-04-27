using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// ������̼� ����� ����ϱ� ���� �߰��ؾ� �ϴ� ���ӽ����̽�
using UnityEngine.AI;


public class MonsterCtrl : MonoBehaviour
{
    // ������ ���� ����
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    // ������ ���� ����
    public State state = State.IDLE;
    // ���� �����Ÿ�
    public float traceDist = 10.0f;
    // ���� �����Ÿ�
    public float attackDist = 2.0f;
    // ������ ��� ����
    public bool isDie = false;

    // ������Ʈ�� ĳ�ø� ó���� ����
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;

    // Animator �Ķ������ �ؽð� ����
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    // ���� ȿ�� ������
    private GameObject bloodEffect;

    // ���� ���� ����
    private int hp = 100;

    // ��ũ��Ʈ�� Ȱ��ȭ�� ������ ȣ�� �Ǵ� �Լ�
    private void OnEnable()
    {
        // �̺�Ʈ �߻� �� ������ �Լ� ����
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;
    }

    // ��ũ��Ʈ�� ��Ȱ��ȭ�� ������ ȣ�� �Ǵ� �Լ�
    private void OnDisable()
    {
        // ������ ����� �Լ� ����
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    private void Start()
    {
        // ���� Transform �Ҵ�
        monsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        // Animator ������Ʈ �Ҵ�
        anim = GetComponent<Animator>();

        // BloodSprayEffect ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(MonsterAction());
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckMonsterState());
    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            // 0.3�� ���� ����(���)�ϴ� ���� ������� �޽��� ������ �纸
            yield return new WaitForSeconds(0.3f);

            // ������ ���°� DIE�� �� �ڷ�ƾ�� ����
            if (state == State.DIE) yield break;
            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
            float  distance = Vector3.Distance(playerTr.position, monsterTr.position);

            // ���� �����Ÿ� ������ ���Դ��� Ȯ��
            if(distance <= attackDist )
            {
                state = State.ATTACK;
            }
            // ���� �����Ÿ� ������ ���Դ��� Ȯ��
            else if(distance <= traceDist )
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }
    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(state)
            {
                // IDLE ����
                case State.IDLE:
                    // ���� ����
                    agent.isStopped = true;

                    // Animator�� IsTrace ������ false�� ����
                    anim.SetBool(hashTrace, false);
                    break;

                case State.TRACE:
                    // ���� ����� ��ǥ�� �̵� ����
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    // Animator�� IsTrace ������ true�� ����
                    anim.SetBool(hashTrace, true);

                    // Animator�� IsAttack ������ false�� ����
                    anim.SetBool(hashAttack, false);
                    break;

                // ���� ����
                case State.ATTACK:
                    // Animtor�� IsAttack ������ true�� ����
                    anim.SetBool(hashAttack, true);
                    break;

                // ���
                case State.DIE:
                    isDie = true;
                    // ���� ����
                    agent.isStopped = true;
                    // ��� �ִϸ��̼� ����
                    anim.SetTrigger(hashDie);
                    // ������ Colider ������Ʈ ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            // �浹�� �Ѿ��� ����
            Destroy(collision.gameObject);
            // �ǰ� ���׼� �ִϸ��̼� ����
            anim.SetTrigger(hashHit);

            // �Ѿ��� �浹 ����
            Vector3 pos = collision.GetContact(0).point;
            // �Ѿ��� �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            // ���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(pos, rot);

            // ������ hp ����
            hp -= 10;
            if(hp <= 0)
            {
                state = State.DIE;
            }
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // ���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);
    }
    private void OnDrawGizmos()
    {
        // ���� �����Ÿ� ǥ��
        if(state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, traceDist);
        }
        // ���� �����Ÿ� ǥ��
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    void OnPlayerDie()
    {
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ��� ��� ������Ŵ
        StopAllCoroutines();

        // ������ �����ϰ� �ִϸ��̼��� ����
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }
}