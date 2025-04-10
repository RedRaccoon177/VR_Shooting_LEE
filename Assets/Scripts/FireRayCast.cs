using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FireRaycast : MonoBehaviour
{
    [Header("레이저 최대 거리 설정")]
    public float fireDistance = 100f; // 레이저 사거리

    [Header("Ray가 충돌할 수 있는 레이어 설정")]
    public LayerMask hitLayers; // 레이저가 맞출 수 있는 오브젝트 레이어 (벽, 병 등)

    [Header("레이저가 나가는 위치 (총구 위치)")]
    public Transform firePoint;  // 레이저 발사 위치

    [Header("맞출 때 얻는 점수")]
    public int scorePerHit = 10; // 병 맞추면 얻는 점수

    [Header("XR 입력 설정 (트리거 버튼 같은거)")]
    public InputActionProperty fireAction; // VR 컨트롤러 입력

    [Header("머즐 플래시 프리팹 (발사 이펙트)")]
    public GameObject muzzleFlashPrefab;

    [Header("라인 렌더러 (레이저 선 그리기)")]
    public LineRenderer lineRenderer;

    [Header("레이저 끝 점 표시 프리팹 (LaserDot)")]
    public GameObject laserDotPrefab;

    private GameObject laserDotInstance; // LaserDot를 복사해서 쓸 오브젝트


    private void Start()
    {
        // 라인 두께 설정 (시작, 끝 동일하게 얇게)
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // 레이저 끝 점 오브젝트 생성
        laserDotInstance = Instantiate(laserDotPrefab);
    }

    private void Update()
    {
        // 레이저는 항상 보여주기 (업데이트)
        UpdateLaser();

        // 발사 입력 처리 (마우스 왼쪽 or VR 트리거 누르면 발사)
        if (Mouse.current.leftButton.wasPressedThisFrame || fireAction.action.WasPressedThisFrame())
        {
            Fire();
        }
    }

    /// <summary>
    /// 레이저 발사 처리 (실제로 병 맞추기)
    /// </summary>
    private void Fire()
    {
        // 머즐 플래시 이펙트 생성
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzle = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(muzzle, 1f); // 1초 후 자동 삭제
        }

        // Ray(광선) 생성 - firePoint 위치에서 forward 방향으로
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        // 기본 레이저 끝 위치 = 최대 거리
        Vector3 laserEndPoint = firePoint.position + firePoint.forward * fireDistance;

        // Ray가 무언가에 닿았을 경우
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, hitLayers))
        {
            Debug.Log("Hit : " + hit.collider.gameObject.name); // 뭐 맞았는지 출력
            laserEndPoint = hit.point; // 레이저 끝을 맞은 위치로 설정

            // 병인지 체크
            GlassBottleTarget bottle = hit.collider.GetComponentInParent<GlassBottleTarget>();
            if (bottle != null)
            {
                bottle.Hit(hit.point); // 병 부수기
            }

            // Scene View에 Ray 그려서 디버깅 확인
            Debug.DrawRay(firePoint.position, laserEndPoint - firePoint.position, Color.red, 1f);
        }
    }

    /// <summary>
    /// 레이저 업데이트 (항상 레이저 선 + 끝 점 보여주기)
    /// </summary>
    private void UpdateLaser()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Vector3 laserEndPoint = firePoint.position + firePoint.forward * fireDistance;

        // Ray 충돌 체크 (벽, 병 포함)
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, hitLayers))
        {
            laserEndPoint = hit.point; // 맞았으면 그 지점으로 끝 설정
        }

        // 레이저 선 그리기 (시작 ~ 끝)
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, laserEndPoint);

        // 레이저 끝 점 위치 이동
        if (laserDotInstance != null)
            laserDotInstance.transform.position = laserEndPoint;
    }
}
