using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type{ Melee,  Range}    //무기타입
    public Type type;
    public int damage;
    public float rate;      //공속
    
    public int maxAmmo;     //전체 탄약
    public int curAmmo;     //현제 탄약

    public BoxCollider melleArea;   //범위
    public TrailRenderer trailEffect;   //휘두를 때 효과 

    //총알, 탄피 관련 변수 생성
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }
    IEnumerator Swing()
    {
        
        yield return new WaitForSeconds(0.1f);
        melleArea.enabled = true;
        trailEffect.enabled = true;
        
        yield return new WaitForSeconds(0.3f);
        melleArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        //#1. 총알 발사
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 100;
        yield return null;
        //#2. 탄피 배출
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10,ForceMode.Impulse);
    }
}
