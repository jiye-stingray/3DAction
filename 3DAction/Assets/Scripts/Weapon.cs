using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type{ Melee,  Range}    //����Ÿ��
    public Type type;
    public int damage;
    public float rate;      //����
    public BoxCollider melleArea;   //����
    public TrailRenderer trailEffect;   //�ֵθ� �� ȿ�� 
    
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
