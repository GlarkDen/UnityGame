using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Часы
/// </summary>
[System.Serializable]
#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
public class Clock
{
    /// <summary>
    /// Секунды
    /// </summary>
    private int seconds;

    /// <summary>
    /// Минуты
    /// </summary>
    private int minutes;

    /// <summary>
    /// Часы
    /// </summary>
    private int hours;

    /// <summary>
    /// Создаёт новые часы
    /// </summary>
    /// <param name="seconds">Секунды</param>
    /// <param name="minutes">Минуты</param>
    /// <param name="hours">Часы</param>
    public Clock(int seconds = 0, int minutes = 0, int hours = 0)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.hours = hours;
    }

    /// <summary>
    /// Секунды
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
    /// Минуты
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
    /// Часы
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
    /// Устанавливает время 00:00
    /// </summary>
    public void Reset()
    {
        seconds = 0;
        minutes = 0;
        hours = 0;
    }

    /// <summary>
    /// Возвращает время в формате 12:30:00
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
    /// Сравнивает время на двух часах
    /// </summary>
    /// <param name="main_cloak">Первые часы</param>
    /// <param name="cloak">Вторые часы</param>
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
    /// Сравнивает время на двух часах
    /// </summary>
    /// <param name="main_cloak">Первые часы</param>
    /// <param name="cloak">Вторые часы</param>
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
[System.Serializable]
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
    /// Создание блока
    /// </summary>
    /// <param name="title">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="texture">Картинка</param>
    /// <param name="type">Тип</param>
    public Block(string title = "", string description = "", byte[] texture = null, byte type = 0)
    {
        this.title = title;
        this.description = description;
        this.texture = texture;
        this.type = type;
    }

    /// <summary>
    /// Описание
    /// </summary>
    public string description;

    /// <summary>
    /// Название
    /// </summary>
    public string title;

    /// <summary>
    /// Текстура
    /// </summary>
    public byte[] texture;

    [NonSerialized]
    /// <summary>
    /// Изображение
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// Тип блока
    /// </summary>
    public byte type;

    /// <summary>
    /// Типы блоков
    /// </summary>
    public static string[] types = new string[4] { "Датчик", "Механизм", "Провод", "Логический блок" };

    /// <summary>
    /// Типы блоков
    /// </summary>
    public static string[] userTypes = new string[2] { "Датчик", "Механизм" };

    /// <summary>
    /// Логические входы и выходы
    /// </summary>
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
