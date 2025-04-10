using TMPro;  // TextMeshPro (글씨 출력용)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Toolkit 관련
using Unity.XR.CoreUtils; // XR Origin 관련
using UnityEngine.SceneManagement; // 씬 변경 (리셋 등)

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴 (GameManager를 전역에서 쉽게 접근 가능하게)
    public static GameManager Instance;

    [Header("플레이어 점수 관련")]
    public int score = 0; // 현재 점수
    public TextMeshProUGUI _textMeshPro; // 점수 표시 UI (TextMeshPro)

    [Header("플레이어 오브젝트")]
    [SerializeField] GameObject _player; // 플레이어 본체 (XR Origin 포함된 오브젝트)

    [Header("유리병 배열 (자동으로 채워짐)")]
    public GlassBottleTarget[] allBottles; // 씬 안에 존재하는 모든 병들

    // 게임 시작할 때 실행되는 함수 (제일 먼저 실행됨)
    private void Awake()
    {
        // 싱글톤 패턴 처리 (Instance가 없으면 내가 Instance가 됨)
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        // 씬 안에 있는 모든 GlassBottleTarget(병) 찾아서 자동 등록
        allBottles = FindObjectsOfType<GlassBottleTarget>();
    }

    /// <summary>
    /// 점수를 추가하는 함수
    /// </summary>
    /// <param name="amount">올릴 점수 양</param>
    public void AddScore(int amount)
    {
        score += amount; // 점수 더하기

        // 점수 UI 업데이트
        _textMeshPro.text = $"Score: {score} / 100";

        // 점수가 100 이상이면 게임 리셋
        if (score >= 100)
        {
            // 현재 활성화된 씬을 다시 로드 (리셋 효과)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// 병 전체 리스폰 (위치 & 상태 초기화)
    /// </summary>
    public void ResetAllBottles()
    {
        foreach (var bottle in allBottles)
        {
            if (bottle != null)
                bottle.RespawnInstant(); // 병 각각 리셋 처리
        }
    }
}
