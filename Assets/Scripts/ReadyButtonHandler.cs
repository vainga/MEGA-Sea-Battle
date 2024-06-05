using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;

public class ReadyButtonHandler : NetworkBehaviour
{
    // ��������� ������ �� GridManager
    public GridManager gridManager;

    public void OnReadyButtonClicked()
    {
        if (isLocalPlayer)
        {
            // ���������, ���� �� ������ �� GridManager
            if (gridManager != null)
            {
                // ��������� ���������� ����������� ������ �� grid
                int filledCellCount = gridManager.grid.OccupiedCells.Count(cell => !cell.IsEmpty);

                // ���������, ��� ���������� ����������� ������ �������� 20
                if (filledCellCount == 20)
                {
                    // ���������� ������ �� ������ ��� ������ � ��������������� ������
                    CmdSetPlayerOccupiedCells(gridManager.grid.OccupiedCells);
                    CmdSetPlayerReady(true);
                }
            }
        }
    }

    [Command]
    private void CmdSetPlayerOccupiedCells(List<Cell> occupiedCells)
    {
        // ���������� ������ (Player1 ��� Player2)
        int player = isServer ? 1 : 2;
        // ������������� ������� ������ ��� ���������������� ������
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
        // ����������, ��� ����� ������ (Player1 ��� Player2)
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
    //    // ���������� ��������� ���������� ������ � GameManager
    //    GameManager.Instance.SetPlayerReady(isReady);
    //}

}
