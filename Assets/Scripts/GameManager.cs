using TMPro;  // TextMeshPro (�۾� ��¿�)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Toolkit ����
using Unity.XR.CoreUtils; // XR Origin ����
using UnityEngine.SceneManagement; // �� ���� (���� ��)

public class GameManager : MonoBehaviour
{
    // �̱��� ���� (GameManager�� �������� ���� ���� �����ϰ�)
    public static GameManager Instance;

    [Header("�÷��̾� ���� ����")]
    public int score = 0; // ���� ����
    public TextMeshProUGUI _textMeshPro; // ���� ǥ�� UI (TextMeshPro)

    [Header("�÷��̾� ������Ʈ")]
    [SerializeField] GameObject _player; // �÷��̾� ��ü (XR Origin ���Ե� ������Ʈ)

    [Header("������ �迭 (�ڵ����� ä����)")]
    public GlassBottleTarget[] allBottles; // �� �ȿ� �����ϴ� ��� ����

    // ���� ������ �� ����Ǵ� �Լ� (���� ���� �����)
    private void Awake()
    {
        // �̱��� ���� ó�� (Instance�� ������ ���� Instance�� ��)
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        // �� �ȿ� �ִ� ��� GlassBottleTarget(��) ã�Ƽ� �ڵ� ���
        allBottles = FindObjectsOfType<GlassBottleTarget>();
    }

    /// <summary>
    /// ������ �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="amount">�ø� ���� ��</param>
    public void AddScore(int amount)
    {
        score += amount; // ���� ���ϱ�

        // ���� UI ������Ʈ
        _textMeshPro.text = $"Score: {score} / 100";

        // ������ 100 �̻��̸� ���� ����
        if (score >= 100)
        {
            // ���� Ȱ��ȭ�� ���� �ٽ� �ε� (���� ȿ��)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// �� ��ü ������ (��ġ & ���� �ʱ�ȭ)
    /// </summary>
    public void ResetAllBottles()
    {
        foreach (var bottle in allBottles)
        {
            if (bottle != null)
                bottle.RespawnInstant(); // �� ���� ���� ó��
        }
    }
}
