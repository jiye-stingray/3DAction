using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float orbitspeed;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        //�׷��� �Ź� ���� 
        transform.position = target.position + offset;

        //RotateAround : Ÿ�� ������ ���ۺ��� ȸ���ϴ� �Լ�
        //��ǥ�� �����̸� �ϱ׷����� ������ ����
        transform.RotateAround(target.position,
                               Vector3.up,
                               orbitspeed * Time.deltaTime);
        //�׷��� �Ź� ����22
        offset = transform.position - target.position;

    }
}
