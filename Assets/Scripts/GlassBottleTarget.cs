using UnityEngine;

/// <summary>
/// ������ ������Ʈ�� ���� ��ũ��Ʈ
/// - ������ �����
/// - ����Ʈ ����
/// - ���� �ð� �� ������
/// - ��� ������ �Լ��� ����
/// </summary>
public class GlassBottleTarget : MonoBehaviour
{
    [Header("������ �ð� ����")]
    public float respawnTime = 5f; // �°� �� �� �ڿ� �ٽ� ��Ÿ���� ����

    [Header("�¾��� �� ���� ����Ʈ ������")]
    public GameObject hitEffectPrefab; // ���� ������ ���� ����Ʈ

    // ���� ��ġ, ȸ���� �����
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // ������ �� ���� ������Ʈ�� (�ڽ� ����)
    private Renderer[] renderers;
    private Collider[] colliders;
    private Rigidbody rb;

    // ������ �� �ѹ��� ���� (�ʱ� ����)
    private void Start()
    {
        // ���� ��ġ, ȸ�� ����
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // �� �ȿ� �ִ� ��� Renderer ������
        renderers = GetComponentsInChildren<Renderer>();

        // �� �ȿ� �ִ� ��� Collider ������
        colliders = GetComponentsInChildren<Collider>();

        // Rigidbody ������ (���� ����)
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ������ �¾��� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="hitPoint">���� ��ġ ��ǥ</param>
    public void Hit(Vector3 hitPoint)
    {
        // ���� �ø���
        GameManager.Instance.AddScore(10);

        // ����Ʈ ����
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(effect, 2f); // 2�� �� ����Ʈ ����
        }

        // ������ ���� (������ �Ⱥ��̰�)
        foreach (var r in renderers)
        {
            r.enabled = false;
        }

        // �ݶ��̴� ���� (������ �ȸ°�)
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        // ���� ���߱�
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // ���� �� ������
        }

        // ���� �ð� �� ������ ���� ����
        Invoke(nameof(Respawn), respawnTime);
    }

    /// <summary>
    /// ���� �ð� �� ����� Respawn (���� ��ġ, ���� ����)
    /// </summary>
    private void Respawn()
    {
        // ���� ��ġ�� �̵�
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // �ٽ� ������ �ѱ�
        foreach (var r in renderers)
        {
            r.enabled = true;
        }

        // �ٽ� �ݶ��̴� �ѱ�
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        // ���� �ٽ� Ȱ��ȭ
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    /// <summary>
    /// ��� ������ ��Ű�� �Լ� (Invoke ��ٸ��� ����)
    /// </summary>
    public void RespawnInstant()
    {
        CancelInvoke(); // ����� Respawn ��� (Hit()���� Invoke �ɸ���)

        // ���� ��ġ�� �̵�
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // ������ & �ݶ��̴� �ѱ�
        foreach (var r in renderers)
        {
            r.enabled = true;
        }
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        // ���� �ٽ� Ȱ��ȭ
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
