using UnityEngine;

public class ClockHand : MonoBehaviour
{
    public TimeInputHandler timeInputHandler; // ������ �� TimeInputHandler
    public bool isHourHand; // �������� �� ������� �������

    private Camera mainCamera; // ������ �� �������� ������
    private Vector3 screenPosition; // ��������� ������� � �������� �����������
    private float angleOffset; // �������� ���� ��� �������
    private Collider2D collider; // ��������� �������

    private bool isDragging; // ���������, ���� �� �������������� �������

    private void Start()
    {
        mainCamera = Camera.main; // �������� ������ �� �������� ������
        collider = GetComponent<Collider2D>(); // �������� ��������� �������
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
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10)); // Z=10 ��� ���������� �����������

            if (collider == Physics2D.OverlapPoint(worldPosition))
            {
                StartDragging(inputPosition);
            }
        }

        // ��������� ���������� ��������������
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false; // ���������� ���� ��������������
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
            UpdateTime(); // ��������� ����� � timeInputField �� ���� ��������
        }
    }

    private void StartDragging(Vector3 inputPosition)
    {
        screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 inputVector = inputPosition - screenPosition;
        angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(inputVector.y, inputVector.x)) * Mathf.Rad2Deg;
        isDragging = true; // ������������� ���� ��������������
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
            timeInputHandler.currentHour = (Mathf.FloorToInt(angle / -15) + 24) % 24; // 360� = 24h
        }
        else
        {
            timeInputHandler.currentMinute = (Mathf.FloorToInt(angle / -6) + 60) % 60; // 360� = 60m
        }

        UpdateTimeInputField();
    }

    private void UpdateTimeInputField()
    {
        // ����������� ����� � ��������� ��������� ����
        timeInputHandler.timeInputField.text = $"{timeInputHandler.currentHour:00}:{timeInputHandler.currentMinute:00}";
    }
    public void ResetClockHands()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); // ������������� ������� � 0 ��������

        // ��������� ������� ����� � TimeInputHandler
        timeInputHandler.currentHour = 0;
        timeInputHandler.currentMinute = 0;
        UpdateTimeInputField(); // ���������� ���� ����� �������
    }
}
