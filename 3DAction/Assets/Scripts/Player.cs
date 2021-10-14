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
    void Start()
    {
        
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis 값을 정수로 변환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : 방향값이 1로 고정된 Vector

        transform.position += moveVec * speed * Time.deltaTime; //transform의 이동은 deltaTime을 반드시 곱해주어야 함
    }
}
