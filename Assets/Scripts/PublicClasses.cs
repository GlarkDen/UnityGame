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

public struct Coordinate
{
    public int X;
    public int Y;

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
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

        timer.text = title + current;

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
    public static void Start(Clock end, float wait = 1, int step = 1, string title = "Время: ", bool reversed = false)
    {
        if (timer == null)
            throw new Exception("TimerNullError");

        Timer.end = end;
        Timer.wait = wait;
        Timer.step = step;
        Timer.title = title;

        if (reversed)
        {
            Timer.step = -step;
            Timer.current = end;
            Timer.end = new Clock();
        }
        else
            Timer.current = new Clock();

        timerClock = timer.StartCoroutine(StartTimer());
    }

    /// <summary>
    /// Остановка таймера
    /// </summary>
    public static void Stop(bool castFunction = true)
    {
        timer.StopCoroutine(timerClock);

        if (castFunction)
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
    public string text;

    /// <summary>
    /// Решение
    /// </summary>
    public Map solution;

    /// <summary>
    /// Количество используемых логических блоков
    /// </summary>
    public int solutionCountBlocks;

    /// <summary>
    /// Управляемый блок
    /// </summary>
    public Block mehanicBlock;

    /// <summary>
    /// Используемые блоки
    /// </summary>
    public List<Block> blocks;

    /// <summary>
    /// Количество каждого из блоков
    /// </summary>
    public List<int> countBlocks;

    /// <summary>
    /// Таблица истинности
    /// </summary>
    public TruthTable truthTable;

    /// <summary>
    /// Размер карты
    /// </summary>
    public int mapSize;
}

[System.Serializable]
/// <summary>
/// Блок
/// </summary>
public class Block : ICloneable
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

    /// <summary>
    /// Типы блоков
    /// </summary>
    public enum Type
    {
        Датчик = 0,
        Механизм = 1,
        Провод = 2,
        Логический_блок = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateSprite()
    {
        sprite = Texture.ByteToSprite(texture, 100, 100);
    }

    public object Clone()
    {
        Block block = new Block(title, description, texture, type);
        block.sprite = sprite;
        return block;
    }
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
    public List<Dictionary<string, bool>> BlockConditions;

    public bool CompareCondition;

    public List<string> BlockChars;

    public bool IsNull = true;

    public bool Compare(TruthTable truthTable)
    {
        if (IsNull && truthTable.IsNull)
            return true;

        if (IsNull || truthTable.IsNull)
            return false;

        if (CompareCondition != truthTable.CompareCondition)
            return false;

        if (BlockConditions.Count != truthTable.BlockConditions.Count)
            return false;

        if (BlockChars.Count != truthTable.BlockChars.Count)
            return false;

        for (int i = 0; i < BlockConditions.Count; i++)
        {
            foreach (string name in BlockChars)
            {
                if (BlockConditions[i][name] != truthTable.BlockConditions[i][name])
                    return false;
            }
        }

        return true;
    }
}

/// <summary>
/// Идеально сбалансированное бинарное дерево
/// </summary>
public class TileBinaryTree
{
    /// <summary>
    /// Информационное поле
    /// </summary>
    public int data;

    /// <summary>
    /// Информационное поле
    /// </summary>
    public string dataName;

    /// <summary>
    /// Ссылка на левую ветку дерева
    /// </summary>
    public TileBinaryTree left;

    /// <summary>
    /// Ссылка на правую ветку дерева
    /// </summary>
    public TileBinaryTree right;

    /// <summary>
    /// Объект с пустыми полями
    /// </summary>
    public TileBinaryTree()
    {

    }

    /// <summary>
    /// Объект с заполненными информационным полем
    /// </summary>
    /// <param name="data">Информационное поле</param>
    public TileBinaryTree(int data)
    {
        this.data = data;
    }

    /// <summary>
    /// Печатает дерево в консоль с отступами
    /// </summary>
    /// <param name="root">Корень дерева</param>
    /// <param name="direction">#Параметр для рекурсии#</param>
    /// <param name="indent">#Параметр для рекурсии#</param>
    public void Show(byte direction = 0, string indent = "", string change_indent = "   ")
    {
        if (direction == 0)
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            Debug.Log(indent + message);

            if (left != null)
                left.Show(2, indent + change_indent, change_indent);

            if (right != null)
                right.Show(1, indent + change_indent, change_indent);
        }
        else if (direction == 1)
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            Debug.Log(indent + message);

            if (left != null)
                left.Show(2, indent + change_indent, change_indent);

            if (right != null)
                right.Show(1, indent + change_indent, change_indent);
        }
        else
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            Debug.Log(indent + message);

            if (right != null)
                right.Show(1, indent + change_indent, change_indent);

            if (left != null)
                left.Show(2, indent + change_indent, change_indent);
        }
    }

    public void MegaShow(Text text_panel, byte direction = 0, string indent = "", string change_indent = "   ")
    {
        if (direction == 0)
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            text_panel.text += indent + message + "\n";

            if (left != null)
                left.MegaShow(text_panel, 2, indent + change_indent, change_indent);

            if (right != null)
                right.MegaShow(text_panel, 1, indent + change_indent, change_indent);
        }
        else if (direction == 1)
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            text_panel.text += indent + message + "\n";

            if (left != null)
                left.MegaShow(text_panel, 2, indent + change_indent, change_indent);

            if (right != null)
                right.MegaShow(text_panel, 1, indent + change_indent, change_indent);
        }
        else
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "Пусто";

            text_panel.text += indent + message + "\n";

            if (right != null)
                right.MegaShow(text_panel, 1, indent + change_indent, change_indent);

            if (left != null)
                left.MegaShow(text_panel, 2, indent + change_indent, change_indent);
        }
    }

    public List<int> GetBlocks(List<int> blocks)
    {
        if (left != null)
            left.GetBlocks(blocks);

        if (right != null)
            right.GetBlocks(blocks);

        if (right == null && left == null)
            if (data != 0)
                blocks.Add(data);

        return blocks;
    }

    public int GetCountLogic(ref int count)
    {
        if (VariablesMechanic.IsLogic(data))
            count++;

        if (left != null)
            left.GetCountLogic(ref count);

        if (right != null)
            right.GetCountLogic(ref count);

        return count;
    }

    public void RenamedData(List<string> blockNames, ref int numberChar)
    {
        if (left != null)
            left.RenamedData(blockNames, ref numberChar);

        if (right != null)
            right.RenamedData(blockNames, ref numberChar);

        if (right == null && left == null)
            if (data != 0)
                dataName = blockNames[numberChar++];
    }

    /// <summary>
    /// Возвращает все элементы дерева ввиде списка (включая корень)
    /// </summary>
    /// <param name="root">Корень дерева</param>
    /// <param name="tree">Ссылка на список</param>
    public List<int> GetData(List<int> tree)
    {
        if (data != 0)
            tree.Add(data);

        if (right != null)
            right.GetData(tree);

        if (left != null)
            left.GetData(tree);

        return tree;
    }

    public bool CheckCondition(Dictionary<string, bool> blockChars)
    {
        if (VariablesMechanic.IsLogic(data))
        {
            switch (data)
            {
                case (int)VariablesMechanic.Logics.And:
                    return right.CheckCondition(blockChars) && left.CheckCondition(blockChars);

                case (int)VariablesMechanic.Logics.Or:
                    return right.CheckCondition(blockChars) || left.CheckCondition(blockChars);

                case (int)VariablesMechanic.Logics.Xor:
                    return right.CheckCondition(blockChars) ^ left.CheckCondition(blockChars);

                case (int)VariablesMechanic.Logics.Not:
                    return !right.CheckCondition(blockChars);

                default:
                    return false;
            }
        }
        else if (VariablesMechanic.IsSensor(data))
        {
            return blockChars[dataName];
        }
        else if (!VariablesMechanic.IsNull(data))
        {
            return right.CheckCondition(blockChars); ;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Возвращает строковое представление информационного поля
    /// </summary>
    /// <returns>data.ToString()</returns>
    public override string ToString()
    {
        return data.ToString();
    }
}

[System.Serializable]
/// <summary>
/// Аккаунты учеников и учителей
/// </summary>
public class Account
{
    /// <summary>
    /// Создание аккаунта
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    public Account(string login, string password, string status)
    {
        this.login = login;
        this.password = password;
        this.status = status;
    }

    /// <summary>
    /// Логин аккаунта
    /// </summary>
    public string login;

    /// <summary>
    /// Пароль аккаунта
    /// </summary>
    public string password;

    /// <summary>
    /// Владелец аккаунта (Учитель, ученик)
    /// </summary>
    public string status;
}

public static class ListExtensions
{
    public static List<T> Clone<T>(this List<T> list) where T : ICloneable
    {
        List<T> cloneList = new List<T>(list.Count);

        foreach (T item in list)
            cloneList.Add((T)item.Clone());

        return cloneList;
    }

    public static List<int> Clone(this List<int> list)
    {
        List<int> cloneList = new List<int>(list.Count);

        foreach (int item in list)
            cloneList.Add(item);

        return cloneList;
    }
}
