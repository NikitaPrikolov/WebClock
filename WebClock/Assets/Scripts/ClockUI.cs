using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ClockUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject timeChangePanel;
    public Transform hourHand, minuteHand, secondHand;
    public Text timeDisplay;

    private DateTime currentRealTime;
    private Coroutine updateClockCoroutine;

    public void UpdateTime(string timeString)
    {
        if (DateTime.TryParse(timeString, out currentRealTime))
        {
            UpdateClockDisplay();
            StartClock();
        }
        else
        {
            LogParsingError();
        }
    }

    public void SynchronizeTime(string newTimeString)
    {
        if (DateTime.TryParse(newTimeString, out currentRealTime))
        {
            currentRealTime = new DateTime(currentRealTime.Year, currentRealTime.Month, currentRealTime.Day,
                                            currentRealTime.Hour, currentRealTime.Minute, 0);
            UpdateClockDisplay();
            StartClock();
        }
        else
        {
            LogParsingError();
        }
    }

    private void StartClock()
    {
        if (updateClockCoroutine != null)
        {
            StopCoroutine(updateClockCoroutine);
        }
        updateClockCoroutine = StartCoroutine(UpdateClockEverySecond());
    }

    private IEnumerator UpdateClockEverySecond()
    {
        while (true)
        {
            currentRealTime = currentRealTime.AddSeconds(1);
            UpdateClockDisplay();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateClockDisplay()
    {
        timeDisplay.text = currentRealTime.ToString("HH:mm:ss");
        UpdateClockHands();
    }

    private void UpdateClockHands()
    {
        hourHand.localRotation = Quaternion.Euler(0, 0, -(currentRealTime.Hour % 12 + currentRealTime.Minute / 60f) * 30f);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -(currentRealTime.Minute + currentRealTime.Second / 60f) * 6f);
        secondHand.localRotation = Quaternion.Euler(0, 0, -currentRealTime.Second * 6f);
    }

    private void LogParsingError() => Debug.LogError("Ошибка при парсинге времени.");
}

