using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����
/// </summary>
[System.Serializable]
#pragma warning disable CS0660 // ��� ���������� �������� == ��� �������� !=, �� �� �������������� Object.Equals(object o)
#pragma warning disable CS0661 // ��� ���������� �������� == ��� �������� !=, �� �� �������������� Object.GetHashCode()
public class Clock
{
    /// <summary>
    /// �������
    /// </summary>
    private int seconds;

    /// <summary>
    /// ������
    /// </summary>
    private int minutes;

    /// <summary>
    /// ����
    /// </summary>
    private int hours;

    /// <summary>
    /// ������ ����� ����
    /// </summary>
    /// <param name="seconds">�������</param>
    /// <param name="minutes">������</param>
    /// <param name="hours">����</param>
    public Clock(int seconds = 0, int minutes = 0, int hours = 0)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.hours = hours;
    }

    /// <summary>
    /// �������
    /// </summary>
    public int Seconds
    {
        get => seconds;
        set
        {
            if (value < 0)
                throw new Exception("SetSecondsError");

            if (value > 59)
            {
                Minutes += value / 60;
                seconds = value % 60;
            }
            else
            {
                seconds = value;
            }
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Minutes
    {
        get => minutes;
        set
        {
            if (value < 0)
                throw new Exception("SetMinuteError");

            if (value > 59)
            {
                Hours += value / 60;
                minutes = value % 60;
            }
            else
            {
                minutes = value;
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public int Hours
    {
        get => hours;
        set
        {
            if (value < 0)
                throw new Exception("SetHoursError");

            if (value > 23)
                throw new Exception("ClockOverflowError");

            hours = value;
        }
    }

    /// <summary>
    /// ������������� ����� 00:00
    /// </summary>
    public void Reset()
    {
        seconds = 0;
        minutes = 0;
        hours = 0;
    }

    /// <summary>
    /// ���������� ����� � ������� 12:30:00
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (seconds == 0)
        {
            if (hours == 0 && minutes == 0)
            {
                return $"0:00";
            }
            else if (minutes == 0)
            {
                return $"{Hours}:00:00";
            }
            else if (hours == 0)
            {
                return $"{Minutes}:00";
            }
            else
            {
                if (Minutes < 10)
                    return $"{Hours}:0{Minutes}:00";

                return $"{Hours}:{Minutes}:00";
            }
        }
        else if (seconds < 10)
        {
            if (hours == 0 && minutes == 0)
            {
                return $"0:0{Seconds}";
            }
            else if (minutes == 0)
            {
                return $"{Hours}:00:0{Seconds}";
            }
            else if (hours == 0)
            {
                return $"{Minutes}:0{Seconds}";
            }
            else
            {
                if (Minutes < 10)
                    return $"{Hours}:0{Minutes}:0{Seconds}";

                return $"{Hours}:{Minutes}:0{Seconds}";
            }
        }
        else
        {
            if (hours == 0 && minutes == 0)
            {
                return $"0:{Seconds}";
            }
            else if (minutes == 0)
            {
                return $"{Hours}:00:{Seconds}";
            }
            else if (hours == 0)
            {
                return $"{Minutes}:{Seconds}";
            }
            else
            {
                if (Minutes < 10)
                    return $"{Hours}:0{Minutes}:{Seconds}";

                return $"{Hours}:{Minutes}:{Seconds}";
            }
        }
    }

    /// <summary>
    /// ���������� ����� �� ���� �����
    /// </summary>
    /// <param name="main_cloak">������ ����</param>
    /// <param name="cloak">������ ����</param>
    public static bool operator ==(Clock main_cloak, Clock cloak)
    {
        if (main_cloak.seconds != cloak.seconds)
            return false;

        if (main_cloak.minutes != cloak.minutes)
            return false;

        if (main_cloak.hours != cloak.hours)
            return false;

        return true;
    }

    /// <summary>
    /// ���������� ����� �� ���� �����
    /// </summary>
    /// <param name="main_cloak">������ ����</param>
    /// <param name="cloak">������ ����</param>
    public static bool operator !=(Clock main_cloak, Clock cloak)
    {
        if (main_cloak.seconds != cloak.seconds)
            return true;

        if (main_cloak.minutes != cloak.minutes)
            return true;

        if (main_cloak.hours != cloak.hours)
            return true;

        return false;
    }
}

/// <summary>
/// ������
/// </summary>
[System.Serializable]
public static class Timer
{
    /// <summary>
    /// ������ ��� ������ �������
    /// </summary>
    public static Text timer;

    /// <summary>
    /// �������� ��� ������� �������
    /// </summary>
    private static Coroutine timerClock;

    /// <summary>
    /// ������� �����
    /// </summary>
    private static Clock current;

    /// <summary>
    /// ����� ���������
    /// </summary>
    private static Clock end;

    /// <summary>
    /// ����� ����� ��������������
    /// </summary>
    private static float wait;

    /// <summary>
    /// ��� ��������� ��������
    /// </summary>
    private static int step;

    /// <summary>
    /// ���������, ������� ���������� ����� ��������
    /// </summary>
    private static string title;

    /// <summary>
    /// ������� �����
    /// </summary>
    public static Clock Current
    {
        get => current;
    }

    /// <summary>
    /// ������������� ��� ��������
    /// </summary>
    private static IEnumerator StartTimer()
    {
        if (step == 0)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(wait);
            current.Seconds += step;

            timer.text = title + current;

            if (end == current)
            {
                Stop();
                yield break;
            }
        }
    }

    /// <summary>
    /// �����, ����������� ��� ��������� �������
    /// </summary>
    public delegate void StopTimer();

    /// <summary>
    /// �����, ����������� ��� ��������� �������
    /// </summary>
    public static StopTimer stopTimer;

    /// <summary>
    /// ������ �������
    /// </summary>
    /// <param name="end">��������� �������</param>
    /// <param name="wait">����� ����� ��������������</param>
    /// <param name="step">��� ��������� ��������</param>
    /// <param name="title">���������, ������� ��������� ����� ��������</param>
    public static void Start(Clock end, float wait = 1, int step = 1, string title = "�����: ")
    {
        if (timer == null)
            throw new Exception("TimerNullError");

        Timer.end = end;
        Timer.wait = wait;
        Timer.step = step;
        Timer.title = title;

        Timer.current = new Clock();

        timerClock = timer.StartCoroutine(StartTimer());
    }

    /// <summary>
    /// ��������� �������
    /// </summary>
    public static void Stop()
    {
        timer.StopCoroutine(timerClock);
        stopTimer();
    }
}

[System.Serializable]
/// <summary>
/// �����
/// </summary>
public class Map
{
    /// <summary>
    /// ������ �����
    /// </summary>
    public int mapSize;

    /// <summary>
    /// ������ ������
    /// </summary>
    public Image[,] mapTiles;

    /// <summary>
    /// ������ ���������� � ������
    /// </summary>
    public int[,] mapTilesValue;
}

[System.Serializable]
/// <summary>
/// ������
/// </summary>
public class Task
{
    /// <summary>
    /// ��������
    /// </summary>
    public string title;

    /// <summary>
    /// ��������
    /// </summary>
    public string description;

    /// <summary>
    /// �������
    /// </summary>
    public Map solution;

    /// <summary>
    /// ��������� �����
    /// </summary>
    public Map startMap;

    /// <summary>
    /// ������������ �����
    /// </summary>
    public string[] blocks;

    /// <summary>
    /// ���������� ������� �� ������
    /// </summary>
    public Dictionary<string, int> countBlocks;

    /// <summary>
    /// ��������������� ����� �� �������
    /// </summary>
    public Clock recomendedTime;
}

[System.Serializable]
/// <summary>
/// ����
/// </summary>
public class Block
{
    /// <summary>
    /// �������� �����
    /// </summary>
    /// <param name="title">��������</param>
    /// <param name="description">��������</param>
    /// <param name="texture">��������</param>
    /// <param name="type">���</param>
    public Block(string title = "", string description = "", byte[] texture = null, byte type = 0)
    {
        this.title = title;
        this.description = description;
        this.texture = texture;
        this.type = type;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public string description;

    /// <summary>
    /// ��������
    /// </summary>
    public string title;

    /// <summary>
    /// ��������
    /// </summary>
    public byte[] texture;

    [NonSerialized]
    /// <summary>
    /// �����������
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// ��� �����
    /// </summary>
    public byte type;

    /// <summary>
    /// ���� ������
    /// </summary>
    public static string[] types = new string[3] { "������", "��������", "������" };

    /// <summary>
    /// ���������� ����� � ������
    /// </summary>
    public static string[] connectionType = new string[3] { "", "In", "Out" };
}

[System.Serializable]
/// <summary>
/// ����� ��������
/// </summary>
public class LearningScheme
{
    /// <summary>
    /// ����� �� �������
    /// </summary>
    public Clock time;

    /// <summary>
    /// ����� �����
    /// </summary>
    public Task[] tasks;

    /// <summary>
    /// ��������
    /// </summary>
    public string title;
}

[System.Serializable]
/// <summary>
/// ������� ����������
/// </summary>
public class TruthTable
{
    /// <summary>
    /// ������
    /// </summary>
    public bool[,] data;

    /// <summary>
    /// ����������� � �������
    /// </summary>
    public override string ToString()
    {
        return data.ToString();
    }
}
