using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    Camera uiCamera;                // Canvas를 랜더링하는 카메라
    Canvas canvas;                  // UI용 최상위 캔버스
    RectTransform rectParent;       // 부모 컴포넌트
    RectTransform rectHp;           // 자식 컴포넌트
    // EnemyHpBar는 UI 항목으로 Canvas 안에 표현
    // --> Canvas 안에서 표현되는 UI 항목은 RectTransform 기준의 좌표를 가지고 있기 때문에 RectTransform 좌표계로 한번 더 변환해야 함
    // 월드좌표 (30,0,100) --> 스크린좌표 (0.2,0.8,0.0) --> Canvas 좌표 (0.12,0.3,0.0)

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    // 적 캐릭터가 이동을 완료한 후 처리하기 위해서 LateUpdate 사용
    void LateUpdate()
    {
        // 월드좌표를 스크린의 좌표로 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        // RectTransform 좌표값을 전달받은 변수
        var localPos = Vector2.zero;
        // 스크린 좌표를 RectTransform 기준의 좌표로 변환
        // ScreenPointToLocalPointInRectangle (부모의 RectTransform, 스크린 좌표, UI 렌더링 카메라, out 변환된 좌표)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        // 생명 게이지 이미지의 위치를 변경
        rectHp.localPosition = localPos;
    }
}
