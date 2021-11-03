using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerMove")]
    //가로세로 입력 받는 함수
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    Vector3 dodgeVec;   //회피도중 방향 전화 놉

    [SerializeField]
    float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenadas;   //공전하는 물체(수류탄)를 컨트롤하기위해서 배열 변수 생성
    public int hasgrenadas;

    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    bool wDown;     //shift 키 인식
    bool jDown;     //점프
    bool iDown;     //상호작용
    bool fDown;     //공격키 입력

    //아이템 교체 키
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isjump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObj; //트리거된 아이템을 저장
    Weapon equipWeapon; //기존에 장착된 변수
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
        hAxis = Input.GetAxisRaw("Horizontal"); //GetAxisRaw() : Axis 값을 정수로 변환하는 함수
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
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //normalized : 방향값이 1로 고정된 Vector

        if (isDodge)
            //회피중에는 움직임 -> 회피 백터로 전환
            moveVec = dodgeVec;
        if (isSwap || !isFireReady)
            moveVec = Vector3.zero;
        

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

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;     //공격딜레이에 시간을 더해주고 공격가능 여부를 확인

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
        //무기 중복 교체 없는 무기 확인을 위한 조건 추가
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
            //빈손일 경우는 생각하여 조건 추가하기
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
        //상호작용 함수가 작동될 수 있는 조건

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
