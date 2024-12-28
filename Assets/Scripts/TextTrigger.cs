using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour
{
    public string message; // Ʈ���� ������ �޽���
    public TMP_Text TriggerText;

    void Start()
    {
        // UI���� ���� ���� �ؽ�Ʈ ã��
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
