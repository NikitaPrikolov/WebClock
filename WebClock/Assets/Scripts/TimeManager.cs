using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
public class TimeManager : MonoBehaviour
{
    private const string API_URL = "https://yandex.com/time/sync.json";
    public GameObject analogClock;
    public GameObject startPanel;
    private DateTime currentTime;
    public ClockUI clockUI;

    private void Start()
    {
        StartCoroutine(InitializeTime());
    }

    public string GetCurrentTime()
    {
        return currentTime.ToString("HH:mm:ss");
    }

    private IEnumerator InitializeTime()
    {
        yield return StartCoroutine(GetRealTime(result =>
        {
            if (result)
            {
                Debug.Log("Время успешно установлено: " + currentTime);
                NotifyClockManager();
                startPanel.SetActive(false);
                analogClock.SetActive(true);
                StartCoroutine(SyncTimeEveryHour()); // Начинаем синхронизацию времени
            }
            else
            {
                Debug.LogError("Не удалось установить время!");
            }
        }));
    }

    private IEnumerator SyncTimeEveryHour()
    {
        while (true)
        {
            yield return new WaitForSeconds(3600); // Ждём 1 час между синхронизациями
            NotifyClockManager();
        }
    }

    private IEnumerator GetRealTime(Action<bool> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка получения времени: {webRequest.error}");
                callback(false);
            }
            else
            {
                long timeInMilliseconds = ParseTime(webRequest.downloadHandler.text);
                currentTime = DateTimeOffset.FromUnixTimeMilliseconds(timeInMilliseconds).DateTime;
                callback(timeInMilliseconds > 0);
            }
        }
    }

    private long ParseTime(string jsonResponse)
    {
        // Извлекаем время из JSON с помощью регулярных выражений
        string timeString = Regex.Match(jsonResponse, @"""time"":\s*(\d+)").Groups[1].Value;
        return long.TryParse(timeString, out long result) ? result : -1;
    }

    private void NotifyClockManager()
    {
        // Обновляем UI с текущим временем, если clockUI инициализирован
        clockUI?.UpdateTime(currentTime.ToString("HH:mm:ss"));
    }
}

