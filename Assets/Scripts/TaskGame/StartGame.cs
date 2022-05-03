using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartGame : MonoBehaviour
{
    float solutionEfficiency;

    string taskText;

    int mapSize = 10;

    float mapScale;
    float border = 10;

    public Image tile;

    public Image mapBackground;

    public Text timer;

    public static Image[,] mapTiles;
    public static int[,] mapTilesValue;

    private void Start()
    {
        mapScale = mapBackground.rectTransform.rect.width;

        mapTiles = new Image[mapSize + 1, mapSize + 1];
        mapTilesValue = new int[mapSize + 1, mapSize + 1];

        float tileSize = (mapScale - border * 2) / mapSize;
        float startPosX = border + tileSize / 2 - mapScale / 2;
        float startPosY = - startPosX + mapBackground.rectTransform.anchoredPosition.y;

        // Генерируем карту
        for (int x = 1; x <= mapSize; x++)
        {
            for (int y = 1; y <= mapSize; y++)
            {
                mapTiles[x, y] = Instantiate(tile, new Vector2(startPosX + (x - 1) * tileSize, startPosY + - (y - 1) * tileSize),
                    Quaternion.identity, mapBackground.transform);

                mapTiles[x, y].rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                mapTiles[x, y].GetComponent<TileManager>().X = x;
                mapTiles[x, y].GetComponent<TileManager>().Y = y;
                mapTiles[x, y].GetComponent<TileManager>().Value = 0;

                mapTilesValue[x, y] = 0;
            }
        }

        tile.gameObject.SetActive(false);

        Timer.timer = timer;
        Timer.stopTimer = StopTimer;
        Timer.Start(new Clock(10));
    }

    public void StopTimer()
    {

    }
}

[System.Serializable]
public class Clock
{
    private int seconds;
    private int minutes;
    private int hours;

    public Clock(int seconds = 0, int minutes = 0, int hours = 0)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.hours = hours;
    }

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

    public void Reset()
    {
        seconds = 0;
        minutes = 0;
        hours = 0;
    }

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
/// Таймер
/// </summary>
public static class Timer
{
    /// <summary>
    /// Объект для вывода времени
    /// </summary>
    public static Text timer;

    /// <summary>
    /// Корутина для запуска таймера
    /// </summary>
    private static Coroutine timerClock;

    /// <summary>
    /// Текущее время
    /// </summary>
    private static Clock current;

    /// <summary>
    /// Время окончания
    /// </summary>
    private static Clock end;

    /// <summary>
    /// Время между срабатываниями
    /// </summary>
    private static float wait;

    /// <summary>
    /// Шаг изменения значения
    /// </summary>
    private static int step;

    /// <summary>
    /// Сообщение, которое печатается перед временем
    /// </summary>
    private static string title;

    /// <summary>
    /// Текущее время
    /// </summary>
    public static Clock Current
    {
        get => current;
    }

    /// <summary>
    /// Перечислитель для корутины
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
    /// Метод, выполняемый при остановке таймера
    /// </summary>
    public delegate void StopTimer();

    /// <summary>
    /// Метод, выполняемый при остановке таймера
    /// </summary>
    public static StopTimer stopTimer;

    /// <summary>
    /// Запуск таймера
    /// </summary>
    /// <param name="end">Остановка таймера</param>
    /// <param name="wait">Время между срабатываниями</param>
    /// <param name="step">Шаг изменения значения</param>
    /// <param name="title">Сообщение, которое выводится перед временем</param>
    public static void Start(Clock end, float wait = 1, int step = 1, string title = "Время: ")
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
    /// Остановка таймера
    /// </summary>
    public static void Stop()
    {
        timer.StopCoroutine(timerClock);
        stopTimer();
    }
}

public class Map
{
    public int mapSize = 10;
    public Image[,] mapTiles;
    public int[,] mapTilesValue;
}