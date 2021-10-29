using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private float lastcoinTime; // 마지막으로 코인이 늘어난 시간
    private float coinGainTime = 1.0f;  // 코인 증가 간격
    private int coinGain = 10; // 코인 증가량

    private void Start()
    {
        coinGain = coinGain * GameManager.instance.currentlevel;
    }

    void Update()
    {
        if (UIMGR.instance.loadingend && Time.time > lastcoinTime + coinGainTime)
        {
            GameManager.instance.AddCoin(coinGain);
            lastcoinTime = Time.time;
        }
        for (int i = 0; i < GameManager.instance.seletedPlayer.Length; i++)
        {
            if (GameManager.instance.seletedPlayer[i] != -1)
            {
                if (GameManager.instance.coin < GameManager.instance.humen[GameManager.instance.seletedPlayer[i]].cost)
                {
                    UIMGR.instance.DisableSlot("Slot" + (i + 1));
                }
                else
                {
                    UIMGR.instance.EnableSlot("Slot" + (i + 1));
                }
            }
        }
    }
}
