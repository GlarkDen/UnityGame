using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesTaskGame : MonoBehaviour
{
    private TruthTable truthTable = new TruthTable();

    private void Start()
    {
        VariablesMechanic.TruthTableUpdate += ChangeTruthTable;
    }

    public void ChangeTruthTable(TruthTable truthTable)
    {
        this.truthTable = truthTable;
    }

    public void CheckSolution()
    {
        bool result = truthTable.Compare(StartTaskGame.GetCurrentTask().truthTable);

        if (result) 
        {
            OnCheckedSolution(true, StartTaskGame.GetCurrentTask().solutionCountBlocks + "/" + VariablesMechanic.SolutionCountBlocks);
        }
        else
        {
            OnCheckedSolution(false);
        }
    }

    public delegate void CheckSolutionHandler(bool result, string efficiency = "");

    public event CheckSolutionHandler CheckedSolution;

    private void OnCheckedSolution(bool result, string efficiency = "")
    {
        if (CheckedSolution != null)
            CheckedSolution(result, efficiency);
    }
}
