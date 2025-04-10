using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FireRaycast : MonoBehaviour
{
    [Header("������ �ִ� �Ÿ� ����")]
    public float fireDistance = 100f; // ������ ��Ÿ�

    [Header("Ray�� �浹�� �� �ִ� ���̾� ����")]
    public LayerMask hitLayers; // �������� ���� �� �ִ� ������Ʈ ���̾� (��, �� ��)

    [Header("�������� ������ ��ġ (�ѱ� ��ġ)")]
    public Transform firePoint;  // ������ �߻� ��ġ

    [Header("���� �� ��� ����")]
    public int scorePerHit = 10; // �� ���߸� ��� ����

    [Header("XR �Է� ���� (Ʈ���� ��ư ������)")]
    public InputActionProperty fireAction; // VR ��Ʈ�ѷ� �Է�

    [Header("���� �÷��� ������ (�߻� ����Ʈ)")]
    public GameObject muzzleFlashPrefab;

    [Header("���� ������ (������ �� �׸���)")]
    public LineRenderer lineRenderer;

    [Header("������ �� �� ǥ�� ������ (LaserDot)")]
    public GameObject laserDotPrefab;

    private GameObject laserDotInstance; // LaserDot�� �����ؼ� �� ������Ʈ


    private void Start()
    {
        // ���� �β� ���� (����, �� �����ϰ� ���)
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // ������ �� �� ������Ʈ ����
        laserDotInstance = Instantiate(laserDotPrefab);
    }

    private void Update()
    {
        // �������� �׻� �����ֱ� (������Ʈ)
        UpdateLaser();

        // �߻� �Է� ó�� (���콺 ���� or VR Ʈ���� ������ �߻�)
        if (Mouse.current.leftButton.wasPressedThisFrame || fireAction.action.WasPressedThisFrame())
        {
            Fire();
        }
    }

    /// <summary>
    /// ������ �߻� ó�� (������ �� ���߱�)
    /// </summary>
    private void Fire()
    {
        // ���� �÷��� ����Ʈ ����
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzle = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(muzzle, 1f); // 1�� �� �ڵ� ����
        }

        // Ray(����) ���� - firePoint ��ġ���� forward ��������
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        // �⺻ ������ �� ��ġ = �ִ� �Ÿ�
        Vector3 laserEndPoint = firePoint.position + firePoint.forward * fireDistance;

        // Ray�� ���𰡿� ����� ���
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, hitLayers))
        {
            Debug.Log("Hit : " + hit.collider.gameObject.name); // �� �¾Ҵ��� ���
            laserEndPoint = hit.point; // ������ ���� ���� ��ġ�� ����

            // ������ üũ
            GlassBottleTarget bottle = hit.collider.GetComponentInParent<GlassBottleTarget>();
            if (bottle != null)
            {
                bottle.Hit(hit.point); // �� �μ���
            }

            // Scene View�� Ray �׷��� ����� Ȯ��
            Debug.DrawRay(firePoint.position, laserEndPoint - firePoint.position, Color.red, 1f);
        }
    }

    /// <summary>
    /// ������ ������Ʈ (�׻� ������ �� + �� �� �����ֱ�)
    /// </summary>
    private void UpdateLaser()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Vector3 laserEndPoint = firePoint.position + firePoint.forward * fireDistance;

        // Ray �浹 üũ (��, �� ����)
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance, hitLayers))
        {
            laserEndPoint = hit.point; // �¾����� �� �������� �� ����
        }

        // ������ �� �׸��� (���� ~ ��)
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, laserEndPoint);

        // ������ �� �� ��ġ �̵�
        if (laserDotInstance != null)
            laserDotInstance.transform.position = laserEndPoint;
    }
}
