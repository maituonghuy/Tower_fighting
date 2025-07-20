using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float delayBeforeStart = 0.2f;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        string[] countdownValues = { "3", "2", "1", "GO!" };
        foreach (string value in countdownValues)
        {
            countdownText.text = value;
            yield return new WaitForSeconds(1f);
        }

        // Load combat scene sau khi đếm xong
     //   SceneManager.LoadScene("FinalCombatScene");
    }
}
