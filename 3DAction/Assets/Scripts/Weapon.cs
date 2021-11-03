using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type{ Melee,  Range}    //무기타입
    public Type type;
    public int damage;
    public float rate;      //공속
    public BoxCollider melleArea;   //범위
    public TrailRenderer trailEffect;   //휘두를 때 효과 
    
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");

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
}
