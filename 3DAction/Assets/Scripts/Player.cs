using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerMove")]
    //���μ��� �Է� �޴� �Լ�
    [SerializeField]
    float hAxis;
    [SerializeField]
    float vAxis;
    Vector3 moveVec;
    [SerializeField]
    float speed;
    bool wDown;     //shift Ű �ν�

    Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis ���� ������ ��ȯ�ϴ� �Լ�
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : ���Ⱚ�� 1�� ������ Vector


        transform.position += moveVec * speed  *(wDown ? 0.3f: 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero); 
        anim.SetBool("isWalk",wDown);

        //�⺻ ȸ�� ����
        transform.LookAt(transform.position + moveVec);
    }
}
