using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour
{
    public Text countdownText;
    [SerializeField] private float countdownDuration = 5f;
    public Coroutine countdownCoroutine;

   
    private void Start()
    {
        //countdownCoroutine = StartCoroutine(StartCountdown());
        
    }
    
    private IEnumerator StartCountdown()
    {
        float currentTime = countdownDuration;

        while (currentTime >= 0)
        {
            countdownText.text = currentTime.ToString("F0");
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
      
        countdownText.text = "";
    }

    public void RestartCountdown()
    {
      
        //if (countdownCoroutine != null)
        //{
        //    StopCoroutine(countdownCoroutine);
        //}
        
        countdownCoroutine = StartCoroutine(StartCountdown());

        
    }
   
}