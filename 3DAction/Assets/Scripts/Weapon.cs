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
    
}
