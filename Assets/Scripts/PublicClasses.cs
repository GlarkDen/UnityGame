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
    public static void Start(Clock end, float wait = 1, int step = 1, string title = "�����: ", bool reversed = false)
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
    public string text;

    /// <summary>
    /// �������
    /// </summary>
    public Map solution;

    /// <summary>
    /// ����������� ����
    /// </summary>
    public Block mehanicBlock;

    /// <summary>
    /// ������������ �����
    /// </summary>
    public Block[] blocks;

    /// <summary>
    /// ���������� ������� �� ������
    /// </summary>
    public int[] countBlocks;

    /// <summary>
    /// ������� ����������
    /// </summary>
    public TruthTable truthTable;
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
    public static string[] types = new string[4] { "������", "��������", "������", "���������� ����" };

    /// <summary>
    /// ���� ������
    /// </summary>
    public static string[] userTypes = new string[2] { "������", "��������" };

    /// <summary>
    /// ���������� ����� � ������
    /// </summary>
    public static string[] connectionType = new string[3] { "", "In", "Out" };

    /// <summary>
    /// ���� ������
    /// </summary>
    public enum Type
    {
        ������ = 0,
        �������� = 1,
        ������ = 2,
        ����������_���� = 3
    }
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
    public List<Dictionary<string, bool>> BlockConditions;

    public bool CompareCondition;

    public List<string> BlockChars;

    public bool isNull;
}

/// <summary>
/// �������� ���������������� �������� ������
/// </summary>
public class TileBinaryTree
{
    /// <summary>
    /// �������������� ����
    /// </summary>
    public int data;

    /// <summary>
    /// �������������� ����
    /// </summary>
    public string dataName;

    /// <summary>
    /// ������ �� ����� ����� ������
    /// </summary>
    public TileBinaryTree left;

    /// <summary>
    /// ������ �� ������ ����� ������
    /// </summary>
    public TileBinaryTree right;

    /// <summary>
    /// ������ � ������� ������
    /// </summary>
    public TileBinaryTree()
    {

    }

    /// <summary>
    /// ������ � ������������ �������������� �����
    /// </summary>
    /// <param name="data">�������������� ����</param>
    public TileBinaryTree(int data)
    {
        this.data = data;
    }

    /// <summary>
    /// �������� ������ � ������� � ���������
    /// </summary>
    /// <param name="root">������ ������</param>
    /// <param name="direction">#�������� ��� ��������#</param>
    /// <param name="indent">#�������� ��� ��������#</param>
    public void Show(byte direction = 0, string indent = "", string change_indent = "   ")
    {
        if (direction == 0)
        {
            string message = "";
            if (data != 0)
                message = data.ToString();
            else
                message = "�����";

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
                message = "�����";

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
                message = "�����";

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
                message = "�����";

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
                message = "�����";

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
                message = "�����";

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
    /// ���������� ��� �������� ������ ����� ������ (������� ������)
    /// </summary>
    /// <param name="root">������ ������</param>
    /// <param name="tree">������ �� ������</param>
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
    /// ���������� ��������� ������������� ��������������� ����
    /// </summary>
    /// <returns>data.ToString()</returns>
    public override string ToString()
    {
        return data.ToString();
    }
}

[System.Serializable]
/// <summary>
/// �������� �������� � ��������
/// </summary>
public class Account
{
    /// <summary>
    /// �������� ��������
    /// </summary>
    /// <param name="login">�����</param>
    /// <param name="password">������</param>
    public Account(string login, string password, string status)
    {
        this.login = login;
        this.password = password;
        this.status = status;
    }

    /// <summary>
    /// ����� ��������
    /// </summary>
    public string login;

    /// <summary>
    /// ������ ��������
    /// </summary>
    public string password;

    /// <summary>
    /// �������� �������� (�������, ������)
    /// </summary>
    public string status;
}
