using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TimeInputHandler : MonoBehaviour
{
    public GameObject saveButton; // ������ ����������
    public int currentHour = 0; // ������� �������� �����
    public int currentMinute = 0;
    public int currentSecond = 0;
    public InputField timeInputField; // ���� ����� �������
    public ClockHand minuteHand;
    public ClockHand hourHand;
    public ClockUI clockUI; // ������ �� ClockUI

    private void Start()
    {
        // �������������� ���� ����� �������
        timeInputField.text = "00:00";
        UpdateSaveButtonState(); // ���������� ��������� ��������� ������
    }

    private void Update()
    {
        ValidateTimeInput();
    }

    public void SaveTime()
    {
        string inputTime = timeInputField.text;

        // ��������� ���������� ������� � ��������� ClockUI
        if (IsValidTime(inputTime))
        {
            // ��������� ����� � ������� "HH:mm:ss"
            clockUI.SynchronizeTime(inputTime + ":00");
        }
        //timeInputField.text = "00:00";
        minuteHand.ResetClockHands();
        hourHand.ResetClockHands();
    }

    private bool IsValidTime(string input)
    {
        // ��������� ������ ������� "HH:mm" � ��������
        if (!Regex.IsMatch(input, @"^\d{2}:\d{2}$"))
            return false;

        // ��������� ������� �� �����
        string[] timeParts = input.Split(':');
        int hour = int.Parse(timeParts[0]);
        int minute = int.Parse(timeParts[1]);

        return hour >= 0 && hour < 24 && minute >= 0 && minute < 60;
    }

    private void ValidateTimeInput()
    {
        string input = timeInputField.text;

        // ���������� ��������, ���� ��� �� ������������� �������
        if (input.Length > 5 || !Regex.IsMatch(input, @"^[0-9:]*$") || (input.Length == 5 && input[2] != ':'))
        {
            timeInputField.text = "00:00"; // ���������� �� ���������� ��������
            UpdateSaveButtonState(); // ��������� ��������� ������
            return;
        }

        // ��������� ���������� ������� ����� �����������
        if (input.Length == 5)
        {
            if (!IsValidTime(input))
            {
                timeInputField.text = "00:00"; // ���������� �� ���������� ��������
            }
        }

        UpdateSaveButtonState(); // ��������� ��������� ������
    }

    private void UpdateSaveButtonState()
    {
        // ��������� ����� ������ � ������ ������ �� ������� ���������
        saveButton.GetComponent<Button>().interactable = (timeInputField.text.Length == 5 && timeInputField.text[2] == ':');
    }
}
