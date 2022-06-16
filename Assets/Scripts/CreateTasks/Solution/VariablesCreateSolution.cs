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

    private bool truthTableUpdated = false;

    private int currentSensorCount = 1;

    void Start()
    {
        TileManagerMechanic.SetBlock += new TileManagerMechanic.SetBlockHandler(SetBlock);
        VariablesMechanic.UpdateSensorCount += new VariablesMechanic.SensorCountHandler(ChangeSensorCount);
        VariablesMechanic.TruthTableUpdate += new VariablesMechanic.TruthTableHandler(TruthTableUpdated);
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

    }

    public void TruthTableUpdated()
    {
        truthTableUpdated = true;
        FinishReady();
    }

    public void SetBlock(int x, int y, int value)
    {
        truthTableUpdated = false;
        FinishReady();
    }

    public void ChangeSensorCount(int count)
    {
        currentSensorCount = count;
        FinishReady();
    }

    public void FinishReady()
    {
        if (truthTableUpdated && currentSensorCount == 0)
        {
            if (!finishCreate.interactable)
                finishCreate.interactable = true;
        }
        else if (finishCreate.interactable)
            finishCreate.interactable = false;
    }
}
