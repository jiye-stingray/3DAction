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
    Vector3 dodgeVec;   //회피도중 방향 전화 놉
    [SerializeField]
    float speed;
    bool wDown;     //shift 키 인식
    bool jDown;     //점프
    bool isjump;
    bool isDodge;

    Rigidbody rigid;
    Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

        GetInput();

        Move();
        Turn();
        Jump();
        Dodge();


    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis 값을 정수로 변환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : 방향값이 1로 고정된 Vector

        if (isDodge)
        {
            //회피중에는 움직임 -> 회피 백터로 전환
            moveVec = dodgeVec;
        }

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //기본 회전 구현
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isjump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 20,ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isjump = true;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isjump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);

        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);

            isjump = false;
        }
    }
}
