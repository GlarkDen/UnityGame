using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTaskGame : MonoBehaviour
{
    public GameObject startObject;

    private static GameObject changeGame;

    public static int CurrentTaskNumber { get; set; }

    public static Text ClockTimer { get; set; }

    public Text timer;

    public Image showSolutionButton;

    public Sprite loadSavingMap;

    public int startTaskNumber;

    public Button checkSolution;

    private bool isSolution = false;

    private Sprite saveSolutionButton;
    private int[,] saveCurrentMap;
    private int[] saveCurrentBlocks;

    public static Task GetCurrentTask()
    {
        return tasks[CurrentTaskNumber];
    }

    public static void ChangeTask(int change)
    {
        if (CurrentTaskNumber + change >= tasks.Count)
            CurrentTaskNumber = CurrentTaskNumber + change - tasks.Count;
        else if (CurrentTaskNumber + change < 0)
            CurrentTaskNumber = tasks.Count + CurrentTaskNumber + change;
        else
            CurrentTaskNumber += change;

        NextTask();
    }

    public static void SetTask(int number)
    {
        CurrentTaskNumber = number;

        NextTask();
    }

    public static List<Task> tasks = Serialization.LoadBinaryFile<List<Task>>(ProjectPath.Tasks);

    void Start()
    {
        ClockTimer = timer;

        startTaskNumber--;
        CurrentTaskNumber = startTaskNumber;

        changeGame = startObject;

        foreach (var task in tasks)
        {
            task.mehanicBlock.CreateSprite();

            foreach (var block in task.blocks)
                block.CreateSprite();
        }

        changeGame.GetComponent<StartGameMechanic>().GenerateMap(tasks[CurrentTaskNumber].mapSize, (Block)tasks[CurrentTaskNumber].mehanicBlock.Clone());
        changeGame.GetComponent<StartGameMechanic>().SetBlockCount(tasks[CurrentTaskNumber].blocks.Clone(), tasks[CurrentTaskNumber].countBlocks.Clone());
        changeGame.GetComponent<StartGameMechanic>().SetTaskText(tasks[CurrentTaskNumber].text);

        Timer.timer = ClockTimer;
        Timer.stopTimer = StopTimer;
        Timer.Start(new Clock(minutes: 10));
    }

    public static void ResetTimer()
    {
        Timer.Stop(false);
        Timer.timer = ClockTimer;
        Timer.stopTimer = StopTimer;
        Timer.Start(new Clock(minutes: 10));
    }

    private static void StopTimer()
    {

    }

    public static void NextTask()
    {
        VariablesMechanic.ClearTruthTable();
        changeGame.GetComponent<StartGameMechanic>().RemoveMap();
        changeGame.GetComponent<StartGameMechanic>().RemoveBlockCount();

        changeGame.GetComponent<StartGameMechanic>().GenerateMap(tasks[CurrentTaskNumber].mapSize, (Block)tasks[CurrentTaskNumber].mehanicBlock.Clone());
        changeGame.GetComponent<StartGameMechanic>().SetBlockCount(tasks[CurrentTaskNumber].blocks.Clone(), tasks[CurrentTaskNumber].countBlocks.Clone());
        changeGame.GetComponent<StartGameMechanic>().SetTaskText(tasks[CurrentTaskNumber].text);

        ResetTimer();
    }

    public void ChangeGameAndSolution()
    {
        if (isSolution)
        {
            isSolution = false;
            checkSolution.interactable = true;
            showSolutionButton.sprite = saveSolutionButton;
            changeGame.GetComponent<StartGameMechanic>().ChangeMap(saveCurrentMap);
            changeGame.GetComponent<StartGameMechanic>().SetBlocksCount(saveCurrentBlocks);
            VariablesMechanic.CreateThuthTableWait();
        }
        else
        {
            checkSolution.interactable = false;
            saveCurrentBlocks = (int[])VariablesMechanic.CountSensorBlocks.Clone();
            saveCurrentMap = (int[,])StartGameMechanic.mapTilesValue.Clone();
            saveSolutionButton = showSolutionButton.sprite;
            showSolutionButton.sprite = loadSavingMap;
            isSolution = true;
            changeGame.GetComponent<StartGameMechanic>().ChangeMap(tasks[CurrentTaskNumber].solution.mapTilesValue);
            changeGame.GetComponent<StartGameMechanic>().ClearBlockCount();
            VariablesMechanic.CreateThuthTableWait();
        }
    }

    public void SolutionToGame()
    {
        if (isSolution)
            ChangeGameAndSolution();
    }

    public void PauseTimer()
    {
        Timer.Pause();
    }

    public void ContinueTimer()
    {
        Timer.Continue();
    }
}
