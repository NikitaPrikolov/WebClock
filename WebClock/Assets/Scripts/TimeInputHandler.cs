using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TimeInputHandler : MonoBehaviour
{
    public GameObject saveButton; // Кнопка сохранения
    public int currentHour = 0; // Текущее значение часов
    public int currentMinute = 0;
    public int currentSecond = 0;
    public InputField timeInputField; // Поле ввода времени
    public ClockHand minuteHand;
    public ClockHand hourHand;
    public ClockUI clockUI; // Ссылка на ClockUI

    private void Start()
    {
        // Инициализируем поле ввода времени
        timeInputField.text = "00:00";
        UpdateSaveButtonState(); // Изначально обновляем состояние кнопки
    }

    private void Update()
    {
        ValidateTimeInput();
    }

    public void SaveTime()
    {
        string inputTime = timeInputField.text;

        // Проверяем валидность времени и обновляем ClockUI
        if (IsValidTime(inputTime))
        {
            // Обновляем время в формате "HH:mm:ss"
            clockUI.SynchronizeTime(inputTime + ":00");
        }
        //timeInputField.text = "00:00";
        minuteHand.ResetClockHands();
        hourHand.ResetClockHands();
    }

    private bool IsValidTime(string input)
    {
        // Проверяем формат времени "HH:mm" и значение
        if (!Regex.IsMatch(input, @"^\d{2}:\d{2}$"))
            return false;

        // Разбиение времени на части
        string[] timeParts = input.Split(':');
        int hour = int.Parse(timeParts[0]);
        int minute = int.Parse(timeParts[1]);

        return hour >= 0 && hour < 24 && minute >= 0 && minute < 60;
    }

    private void ValidateTimeInput()
    {
        string input = timeInputField.text;

        // Сбрасываем значение, если оно не соответствует формату
        if (input.Length > 5 || !Regex.IsMatch(input, @"^[0-9:]*$") || (input.Length == 5 && input[2] != ':'))
        {
            timeInputField.text = "00:00"; // Сбрасываем на корректное значение
            UpdateSaveButtonState(); // Обновляем состояние кнопки
            return;
        }

        // Проверяем валидность времени перед обновлением
        if (input.Length == 5)
        {
            if (!IsValidTime(input))
            {
                timeInputField.text = "00:00"; // Сбрасываем на корректное значение
            }
        }

        UpdateSaveButtonState(); // Обновляем состояние кнопки
    }

    private void UpdateSaveButtonState()
    {
        // Проверяем длину текста и третий символ на наличие двоеточия
        saveButton.GetComponent<Button>().interactable = (timeInputField.text.Length == 5 && timeInputField.text[2] == ':');
    }
}
