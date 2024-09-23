using UnityEngine;

public class ClockHand : MonoBehaviour
{
    public TimeInputHandler timeInputHandler; // Ссылка на TimeInputHandler
    public bool isHourHand; // Является ли стрелка часовой

    private Camera mainCamera; // Ссылка на основную камеру
    private Vector3 screenPosition; // Положение стрелки в экранных координатах
    private float angleOffset; // Смещение угла при нажатии
    private Collider2D collider; // Коллайдер стрелки

    private bool isDragging; // Указывает, идет ли перетаскивание стрелки

    private void Start()
    {
        mainCamera = Camera.main; // Получаем ссылку на основную камеру
        collider = GetComponent<Collider2D>(); // Получаем коллайдер стрелки
    }

    private void Update()
    {
        HandleMouseInput();
        HandleDragging();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 inputPosition = GetInputPosition();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10)); // Z=10 для правильной конвертации

            if (collider == Physics2D.OverlapPoint(worldPosition))
            {
                StartDragging(inputPosition);
            }
        }

        // Обработка завершения перетаскивания
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false; // Сбрасываем флаг перетаскивания
        }
    }

    private void HandleDragging()
    {
        if (isDragging)
        {
            Vector3 inputPosition = GetInputPosition();
            Vector3 inputVector = inputPosition - screenPosition;
            float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);
            UpdateTime(); // Обновляем время в timeInputField по углу поворота
        }
    }

    private void StartDragging(Vector3 inputPosition)
    {
        screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 inputVector = inputPosition - screenPosition;
        angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(inputVector.y, inputVector.x)) * Mathf.Rad2Deg;
        isDragging = true; // Устанавливаем флаг перетаскивания
    }

    private Vector3 GetInputPosition()
    {
        return (Input.touchCount > 0) ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
    }

    private void UpdateTime()
    {
        float angle = transform.rotation.eulerAngles.z;

        if (isHourHand)
        {
            timeInputHandler.currentHour = (Mathf.FloorToInt(angle / -15) + 24) % 24; // 360° = 24h
        }
        else
        {
            timeInputHandler.currentMinute = (Mathf.FloorToInt(angle / -6) + 60) % 60; // 360° = 60m
        }

        UpdateTimeInputField();
    }

    private void UpdateTimeInputField()
    {
        // Форматируем время и обновляем текстовое поле
        timeInputHandler.timeInputField.text = $"{timeInputHandler.currentHour:00}:{timeInputHandler.currentMinute:00}";
    }
    public void ResetClockHands()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); // Устанавливаем поворот в 0 градусов

        // Обновляем текущее время в TimeInputHandler
        timeInputHandler.currentHour = 0;
        timeInputHandler.currentMinute = 0;
        UpdateTimeInputField(); // Обновление поля ввода времени
    }
}
