using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour
{
    public string message; // 트리거 영역의 메시지
    public TMP_Text TriggerText;

    void Start()
    {
        // UI에서 내용 넣을 텍스트 찾기
        TriggerText = GameObject.Find("ControllTutorial").GetComponent<TMP_Text>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerText.text = message;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerText.text = "";
        }
    }
}
