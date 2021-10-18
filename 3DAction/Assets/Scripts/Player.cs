using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerMove")]
    //가로세로 입력 받는 함수
    [SerializeField]
    float hAxis;
    [SerializeField]
    float vAxis;
    Vector3 moveVec;
    [SerializeField]
    float speed;
    bool wDown;     //shift 키 인식

    Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis 값을 정수로 변환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : 방향값이 1로 고정된 Vector


        transform.position += moveVec * speed  *(wDown ? 0.3f: 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero); 
        anim.SetBool("isWalk",wDown);

        //기본 회전 구현
        transform.LookAt(transform.position + moveVec);
    }
}
