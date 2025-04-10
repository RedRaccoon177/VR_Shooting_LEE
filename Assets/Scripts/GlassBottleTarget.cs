using UnityEngine;

/// <summary>
/// 유리병 오브젝트에 들어가는 스크립트
/// - 맞으면 사라짐
/// - 이펙트 나옴
/// - 일정 시간 뒤 리스폰
/// - 즉시 리스폰 함수도 존재
/// </summary>
public class GlassBottleTarget : MonoBehaviour
{
    [Header("리스폰 시간 설정")]
    public float respawnTime = 5f; // 맞고 몇 초 뒤에 다시 나타날지 설정

    [Header("맞았을 때 나올 이펙트 프리팹")]
    public GameObject hitEffectPrefab; // 유리 깨지는 느낌 이펙트

    // 원래 위치, 회전값 저장용
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // 유리병 모델 관련 컴포넌트들 (자식 포함)
    private Renderer[] renderers;
    private Collider[] colliders;
    private Rigidbody rb;

    // 시작할 때 한번만 실행 (초기 세팅)
    private void Start()
    {
        // 현재 위치, 회전 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 병 안에 있는 모든 Renderer 가져옴
        renderers = GetComponentsInChildren<Renderer>();

        // 병 안에 있는 모든 Collider 가져옴
        colliders = GetComponentsInChildren<Collider>();

        // Rigidbody 가져옴 (있을 때만)
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 레이저 맞았을 때 실행되는 함수
    /// </summary>
    /// <param name="hitPoint">맞은 위치 좌표</param>
    public void Hit(Vector3 hitPoint)
    {
        // 점수 올리기
        GameManager.Instance.AddScore(10);

        // 이펙트 생성
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(effect, 2f); // 2초 후 이펙트 삭제
        }

        // 렌더러 끄기 (유리병 안보이게)
        foreach (var r in renderers)
        {
            r.enabled = false;
        }

        // 콜라이더 끄기 (유리병 안맞게)
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        // 물리 멈추기
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // 물리 안 먹히게
        }

        // 일정 시간 뒤 리스폰 실행 예약
        Invoke(nameof(Respawn), respawnTime);
    }

    /// <summary>
    /// 일정 시간 뒤 실행될 Respawn (원래 위치, 상태 복구)
    /// </summary>
    private void Respawn()
    {
        // 원래 위치로 이동
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // 다시 렌더러 켜기
        foreach (var r in renderers)
        {
            r.enabled = true;
        }

        // 다시 콜라이더 켜기
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        // 물리 다시 활성화
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    /// <summary>
    /// 즉시 리스폰 시키는 함수 (Invoke 기다리지 않음)
    /// </summary>
    public void RespawnInstant()
    {
        CancelInvoke(); // 예약된 Respawn 취소 (Hit()에서 Invoke 걸린거)

        // 원래 위치로 이동
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // 렌더러 & 콜라이더 켜기
        foreach (var r in renderers)
        {
            r.enabled = true;
        }
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        // 물리 다시 활성화
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
