using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        // ���� Transform �Ҵ�
        monsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        //// ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
        //agent.SetDestination(playerTr.position);

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
                    break;
                case State.TRACE:
                    // ���� ����� ��ǥ�� �̵� ����
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    break;

                // ���� ����
                case State.ATTACK:
                    break;

                // ���
                case State.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
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
}