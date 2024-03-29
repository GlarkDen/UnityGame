using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentObjectMechanic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image showObject;
    private bool collision = false;

    private void Start()
    {
        VariablesMechanic.ShowCurrentObject = showObject;
        showObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (collision)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                VariablesMechanic.SetCurrentBlock("swip");
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                int CurrentTileValue = VariablesMechanic.CurrentTile.GetComponent<TileManagerMechanic>().Value;

                if (CurrentTileValue > 0)
                {
                    int x = VariablesMechanic.CurrentTile.GetComponent<TileManagerMechanic>().X;
                    int y = VariablesMechanic.CurrentTile.GetComponent<TileManagerMechanic>().Y;

                    OnRemoveTile(x, y, VariablesMechanic.CurrentTile.GetComponent<TileManagerMechanic>().Value);

                    VariablesMechanic.CurrentTile.GetComponent<TileManagerMechanic>().Value = 0;
                    StartGameMechanic.mapTilesValue[x, y] = 0;
                    VariablesMechanic.CurrentTile.sprite = VariablesMechanic.Sprites[0];

                    if (VariablesMechanic.IsSensor(CurrentTileValue))
                    {
                        VariablesMechanic.UpdateCountSensors(CurrentTileValue, 1);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.R))
                VariablesMechanic.ChooseBlockButtons("R");

            if (Input.GetKeyUp(KeyCode.V))
                VariablesMechanic.ChooseBlockButtons("V");

            if (Input.GetKeyUp(KeyCode.C))
                VariablesMechanic.ChooseBlockButtons("C");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        collision = true;

        if ((VariablesMechanic.CurrentBlock != 0) && (VariablesMechanic.CurrentBlockActive))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            showObject.transform.position = new Vector2(pos.x, pos.y);
            showObject.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        collision = false;

        showObject.gameObject.SetActive(false);
    }

    public delegate void RemoveTileHandler(int x, int y, int value);

    public static event RemoveTileHandler RemoveTile;

    private void OnRemoveTile(int x, int y, int value)
    {
        if (RemoveTile != null)
            RemoveTile(x, y, value);
    }
}
