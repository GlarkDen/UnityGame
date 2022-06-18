using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesCreateSolution : MonoBehaviour
{
    public GameObject taskOptionsStage;

    public GameObject createSolutionStage;

    public GameObject restartGame;

    public Button finishCreate;

    private TruthTable truthTable;
    private int currentSensorCount = 1;

    private List<Task> tasks = Serialization.LoadBinaryFile<List<Task>>(ProjectPath.Tasks);

    void Start()
    {
        VariablesMechanic.UpdateSensorCount += new VariablesMechanic.SensorCountHandler(ChangeSensorCount);
        VariablesMechanic.TruthTableUpdate += new VariablesMechanic.TruthTableHandler(TruthTableUpdated);

        TileManagerMechanic.SetBlock += TileMapChanged;
        CurrentObjectMechanic.RemoveTile += TileMapChanged;
    }

    public void BackStage()
    {
        finishCreate.interactable = false;
        taskOptionsStage.SetActive(true);
        createSolutionStage.SetActive(false);

        VariablesMechanic.ClearTruthTable();
        restartGame.GetComponent<StartGameMechanic>().RemoveMap();
        restartGame.GetComponent<StartGameMechanic>().RemoveBlockCount();
    }

    public void FinishCreate()
    {
        Task createTask = new Task();

        createTask.mehanicBlock = StartGameMechanic.MehanicBlock;
        createTask.text = VariablesCreateTasks.taskText;
        createTask.title = VariablesCreateTasks.taskTitle;
        createTask.solution = restartGame.GetComponent<StartGameMechanic>().SaveMap();
        createTask.truthTable = truthTable;
        createTask.blocks = VariablesCreateTasks.GetBlocksList();
        createTask.countBlocks = VariablesCreateTasks.GetBlocksCount();
        createTask.mapSize = VariablesCreateTasks.sizeMap;
        createTask.solutionCountBlocks = VariablesMechanic.SolutionCountBlocks;

        if (tasks == null)
            tasks = new List<Task>();
        
        tasks.Add(createTask);

        Serialization.SaveBinaryFile(tasks, ProjectPath.Tasks);
    }

    public void TruthTableUpdated(TruthTable truthTable)
    {
        this.truthTable = truthTable;
        FinishReady();
    }

    public void ChangeSensorCount(int count)
    {
        currentSensorCount = count;
        FinishReady();
    }

    public void TileMapChanged(int x, int y, int value)
    {
        finishCreate.interactable = false;
    }

    public void FinishReady()
    {
        if (currentSensorCount == 0)
        {
            if (!finishCreate.interactable)
                finishCreate.interactable = true;
        }
        else if (finishCreate.interactable)
            finishCreate.interactable = false;
    }
}
