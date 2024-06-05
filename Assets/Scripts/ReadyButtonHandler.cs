using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;

public class ReadyButtonHandler : NetworkBehaviour
{
    // Добавляем ссылку на GridManager
    public GridManager gridManager;

    public void OnReadyButtonClicked()
    {
        if (isLocalPlayer)
        {
            // Проверяем, есть ли ссылка на GridManager
            if (gridManager != null)
            {
                // Проверяем количество заполненных клеток на grid
                int filledCellCount = gridManager.grid.OccupiedCells.Count(cell => !cell.IsEmpty);

                // Проверяем, что количество заполненных клеток достигло 20
                if (filledCellCount == 20)
                {
                    // Отправляем данные на сервер для записи в соответствующие списки
                    CmdSetPlayerOccupiedCells(gridManager.grid.OccupiedCells);
                    CmdSetPlayerReady(true);
                }
            }
        }
    }

    [Command]
    private void CmdSetPlayerOccupiedCells(List<Cell> occupiedCells)
    {
        // Определяем игрока (Player1 или Player2)
        int player = isServer ? 1 : 2;
        // Устанавливаем занятые клетки для соответствующего игрока
        GameManager.Instance.SetPlayerOccupiedCells(player, occupiedCells);
    }


    //public void OnReadyButtonClicked()
    //{
    //    if (isLocalPlayer)
    //    {
    //        CmdSetPlayerReady(true);
    //    }
    //}

    [Command]
    private void CmdSetPlayerReady(bool isReady)
    {
        RpcSetPlayerReady(isReady);
    }

    [ClientRpc]
    private void RpcSetPlayerReady(bool isReady)
    {
        // Определяем, кто нажал кнопку (Player1 или Player2)
        int player = isServer ? 1 : 2;
        GameManager.Instance.SetPlayerReady(player, isReady);
    }
    //public void OnReadyButtonClicked()
    //{
    //    if (isLocalPlayer)
    //    {
    //        CmdSetPlayerReady(true);
    //    }
    //}

    //[Command]
    //private void CmdSetPlayerReady(bool isReady)
    //{
    //    RpcSetPlayerReady(isReady);
    //}

    //[ClientRpc]
    //private void RpcSetPlayerReady(bool isReady)
    //{
    //    // Установить состояние готовности игрока в GameManager
    //    GameManager.Instance.SetPlayerReady(isReady);
    //}

}
