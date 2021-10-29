using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

public class Enemy : LivingEntity
{
    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리

    private Animator enemyAnimator; // 에너미의 애니메이터

    // 생명 게이지의 프리팹을 저장할 변수
    public GameObject hpBarPrefab;
    // 생명 게이지의 위치를 보정할 오프셋
    public Vector3 hpBarOffeset = new Vector3(0, 1.8f, 0);
    // 부모가 될 Canvas 객체
    Canvas uiCanvas;
    // 생명 수치에 따라 fillAmount 속성을 변경할 image
    Image hpBarImage;
    // 생성된 생명 게이지 UI
    private GameObject hpBar;
    // 공격 대상
    public LivingEntity attackTarget;

    private float lastAttackTime; // 마지막 공격 시점

    float tempTime;

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Move(currentSpeed);

        if (attackTarget != null && attackTarget.dead && !dead)
        {
            Move(speed);
        }
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
        // 생명 게이지의 생성 및 초기화
        SetHpBar();
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        // 체력이 변하면 HPBar 표시
        StartCoroutine(HealthDisplay(health));
    }

    // 데미지 처리
    public override void OnDamage(float damage)
    {
        if (!dead)
        {
            GameObject.Find("SFXSound").GetComponent<SFXSound>().PlaySFX(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage);
        // 체력이 변하면 HPBar 표시
        StartCoroutine(HealthDisplay(health));
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();
        Destroy(hpBar.gameObject, 2.5f);

        GameObject.Find("SFXSound").GetComponent<SFXSound>().PlaySFX(deathClip);
        //enemyAnimator.SetTrigger("Die");
    }

    // 체력 표시
    private IEnumerator HealthDisplay(float currentHealth)
    {
        hpBar.gameObject.SetActive(true);
        hpBarImage.fillAmount = currentHealth / startingHealth;

        yield return new WaitForSeconds(2f);

        hpBar.gameObject.SetActive(false);
    }

    // 초기 체력바 설정
    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas 하위로 생명 게이지를 생성
        hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        // fillAmount 속성을 변경할 Image를 추출
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        // 생명 게이지가 따라가야할 대상과 오프셋 값 설정
        var _hpBar = hpBar.GetComponent<HpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffeset;
        hpBar.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 Player이라면 공격 실행
        if (!dead && collision.tag == "Player" && !collision.isTrigger)
        {
            Move(0f);
            attackTarget = collision.GetComponent<LivingEntity>();
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                if (attackTarget != null)
                {
                    attackTarget.OnDamage(damage);
                }
                else
                {
                    collision.GetComponent<Tower_Damage>().OnDamage(damage);
                }
                lastAttackTime = Time.time;
            }
        }
    }
}
