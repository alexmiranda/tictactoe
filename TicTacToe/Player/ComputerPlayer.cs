using System;
using TicTacToe.Games;

namespace TicTacToe.Player
{
    internal class ComputerPlayer : BasePlayer
    {
        public ComputerPlayer(string name, Mark mark) : base(name, mark)
        {
        }

        public override void OnNext(IGame value)
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
