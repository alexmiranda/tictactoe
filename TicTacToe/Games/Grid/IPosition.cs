using System.Diagnostics.Contracts;

namespace TicTacToe.Games.Grid
{
    [ContractClass(typeof(PositionContracts))]
    public interface IPosition
    {
        int Row { get; }
        int Col { get; }
    }
}