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
        //그래서 매번 갱신 
        transform.position = target.position + offset;

        //RotateAround : 타겟 주위를 빙글빙글 회전하는 함수
        //목표가 움직이면 일그러지는 단점이 있음
        transform.RotateAround(target.position,
                               Vector3.up,
                               orbitspeed * Time.deltaTime);
        //그래서 매번 갱신22
        offset = transform.position - target.position;

    }
}
