using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    //�浹�� ������ �� �߻��ϴ� �̺�Ʈ
    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� ���ӿ�����Ʈ�� �±װ� ��
        if (collision.collider.CompareTag("Bullet"))
        {
            //�浹�� ������Ʈ ����
            Destroy(gameObject);
        }
    }
}
