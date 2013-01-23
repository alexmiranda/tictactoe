using System;
using System.Diagnostics.Contracts;
using TicTacToe.Games.Grid;

namespace TicTacToe.Games
{
    public sealed class Move : IEquatable<Move>
    {
        internal Move(Mark mark, IPosition position)
        {
            Contract.Requires<ArgumentException>(mark != Mark.Blank, "Mark cannot be blank");
            Contract.Requires<ArgumentNullException>(position != null, "position");

            Mark = mark;
            Position = position;
        }

        public Mark Mark { get; private set; }
        public IPosition Position { get; private set; }

        public bool Equals(Move other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Mark.Equals(other.Mark) && Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Move && Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Mark.GetHashCode()*397) ^ Position.GetHashCode();
            }
        }

        public static bool operator ==(Move left, Move right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !Equals(left, right);
        }
    }
}
