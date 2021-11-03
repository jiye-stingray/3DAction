using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerMove")]
    //���μ��� �Է� �޴� �Լ�
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    Vector3 dodgeVec;   //ȸ�ǵ��� ���� ��ȭ ��

    [SerializeField]
    float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenadas;   //�����ϴ� ��ü(����ź)�� ��Ʈ���ϱ����ؼ� �迭 ���� ����
    public int hasgrenadas;

    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    bool wDown;     //shift Ű �ν�
    bool jDown;     //����
    bool iDown;     //��ȣ�ۿ�
    bool fDown;     //����Ű �Է�

    //������ ��ü Ű
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isjump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObj; //Ʈ���ŵ� �������� ����
    Weapon equipWeapon; //������ ������ ����
    int equipWeaponIndex = -1;
    float fireDelay;

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
        Attack();
        Dodge();
        Iteraction();
        Swap();

    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis ���� ������ ��ȯ�ϴ� �Լ�
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Iteraction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : ���Ⱚ�� 1�� ������ Vector

        if (isDodge)
            //ȸ���߿��� ������ -> ȸ�� ���ͷ� ��ȯ
            moveVec = dodgeVec;
        if (isSwap || !isFireReady)
            moveVec = Vector3.zero;
        

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        //�⺻ ȸ�� ����
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

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;     //���ݵ����̿� �ð��� �����ְ� ���ݰ��� ���θ� Ȯ��

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isjump && !isDodge &&!isSwap)
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

    void Swap()
    {
        //���� �ߺ� ��ü ���� ���� Ȯ���� ���� ���� �߰�
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if((sDown1 || sDown2 || sDown3) &&!isDodge &&!isjump)
        {
            //����� ���� �����Ͽ� ���� �߰��ϱ�
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
            
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Iteraction()
    {
        //��ȣ�ۿ� �Լ��� �۵��� �� �ִ� ����

        if (iDown && nearObj != null && !isjump && !isDodge)
        {
            if (nearObj.tag == "Weapon")
            {
                Item item = nearObj.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObj);


            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);

            isjump = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    this.ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    this.coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Grenade:

                    if (hasGrenades == maxHasGrenades)
                        return;
                    grenadas[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
                case Item.Type.Heart:
                    this.health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
              
                default:
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObj = other.gameObject;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObj = null;
    }
}
