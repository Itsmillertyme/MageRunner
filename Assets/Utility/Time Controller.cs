using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float slowTimeScale;
    [SerializeField] KeyCode slowTimeKeyCode;
    private bool isTimeRunning = true;
    private bool isTimeSlowed = false;

    private void Update()
    {
        if (Input.GetKeyDown(slowTimeKeyCode))
        {
            if (isTimeSlowed)
            {
                ResumeNormalTime();
            }
            else
            {
                SlowTime();
            }
        }
    }

    private void SlowTime()
    {
        Time.timeScale = slowTimeScale;
        isTimeSlowed = true;
    }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
        isTimeRunning = false;
    }

    private void ResumeNormalTime()
    {
        Time.timeScale = 1f;
        isTimeRunning = true;
        isTimeSlowed = false;
    }
}