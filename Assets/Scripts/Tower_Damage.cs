using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Damage : MonoBehaviour
{
    float initHp = 200.0f;
    public float currHp;
    public Image hpBar;

    //생명 게이지의 처음 색상(녹색)
    readonly Color initColor = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
    Color currColor;

    public AudioClip damageClip;

    void Start()
    {
        if (gameObject.tag == "EnemyTower")
        {
            currHp = initHp * (GameManager.instance.currentlevel+1)/2;
        }
        else
        {
            currHp = initHp;
        }
        hpBar.color = initColor;
        currColor = initColor;
    }

    void DisPlayHpBar()
    {
        //생명 수치가 50%일때 까지는 녹색에서 노란색으로 변경
        if ((currHp / initHp) > 0.5f)
        {
            currColor.r = (1 - (currHp / initHp)) * 2.0f;
        }
        else    //생명 수치가 0% 일때 까지는 노란색에서 빨간색으로 변경
        {
            currColor.g = (currHp / initHp) * 2.0f;
        }
        //Hpbar의 색상 변경
        hpBar.color = currColor;
        //Hpbar의 크기 변경
        hpBar.fillAmount = (currHp / initHp);
    }

    public void OnDamage(float damage)
    {
        currHp -= damage;
        DisPlayHpBar();
        GameObject.Find("SFXSound").GetComponent<SFXSound>().PlaySFX(damageClip);
        if (currHp <= 0)
        {
            if(gameObject.tag == "Player")
            {
                UIMGR.instance.GameOverUI();
            }
            else if(gameObject.tag == "EnemyTower")
            {
                UIMGR.instance.GameClearUI();
                if(GameManager.instance.clearlevel < GameManager.instance.currentlevel)
                {
                    GameManager.instance.clearlevel++;
                }
            }
        }
    }
}
