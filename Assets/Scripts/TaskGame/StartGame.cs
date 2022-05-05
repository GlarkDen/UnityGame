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

    public Image[,] mapTiles;
    public int[,] mapTilesValue;

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
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
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

[System.Serializable]
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

[System.Serializable]
/// <summary>
/// Карта
/// </summary>
public class Map
{
    /// <summary>
    /// Размер карты
    /// </summary>
    public int mapSize;

    /// <summary>
    /// Массив тайлов
    /// </summary>
    public Image[,] mapTiles;
    
    /// <summary>
    /// Массив информации о тайлах
    /// </summary>
    public int[,] mapTilesValue;
}

[System.Serializable]
/// <summary>
/// Задача
/// </summary>
public class Task
{
    /// <summary>
    /// Название
    /// </summary>
    public string title;

    /// <summary>
    /// Описание
    /// </summary>
    public string description;

    /// <summary>
    /// Решение
    /// </summary>
    public Map solution;
    
    /// <summary>
    /// Стартовая карта
    /// </summary>
    public Map startMap;

    /// <summary>
    /// Используемые блоки
    /// </summary>
    public string[] blocks;

    /// <summary>
    /// Количество каждого из блоков
    /// </summary>
    public Dictionary<string, int> countBlocks;

    /// <summary>
    /// Рекомендованное время на решение
    /// </summary>
    public Clock recomendedTime;
}

[System.Serializable]
/// <summary>
/// Блок
/// </summary>
public class Block
{
    /// <summary>
    /// Таблица истинности
    /// </summary>
    public TruthTable truthTable;

    /// <summary>
    /// Описание
    /// </summary>
    public string description;

    /// <summary>
    /// Название
    /// </summary>
    public string title;

    /// <summary>
    /// Логическая карта блока
    /// </summary>
    public Map map;

    /// <summary>
    /// Название изображения
    /// </summary>
    public string textureName;

    /// <summary>
    /// Тип блока
    /// </summary>
    public byte type;

    public static string[] types = new string[3] { "Датчик", "Механизм", "Какой-то тип" };

    public static string[] connectionType = new string[3] { "", "In", "Out" };
}

[System.Serializable]
/// <summary>
/// Схема обучения
/// </summary>
public class LearningScheme
{
    /// <summary>
    /// Время на решение
    /// </summary>
    public Clock time;

    /// <summary>
    /// Набор задач
    /// </summary>
    public Task[] tasks;

    /// <summary>
    /// Название
    /// </summary>
    public string title;
}

[System.Serializable]
/// <summary>
/// Таблица истинности
/// </summary>
public class TruthTable
{
    /// <summary>
    /// Данные
    /// </summary>
    public bool[,] data;

    /// <summary>
    /// Конвертация в таблицу
    /// </summary>
    public override string ToString()
    {
        return data.ToString();
    }
}