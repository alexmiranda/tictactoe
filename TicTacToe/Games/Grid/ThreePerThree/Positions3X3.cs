namespace TicTacToe.Games.Grid.ThreePerThree
{
    public sealed class Positions3X3 : IPosition
    {
        // Corners
        public static readonly Positions3X3 TopLeftCorner = new Positions3X3 { Row = 0, Col = 0 };
        public static readonly Positions3X3 TopRightCorner = new Positions3X3 { Row = 0, Col = 2 };
        public static readonly Positions3X3 BottomLeftCorner = new Positions3X3 { Row = 2, Col = 0 };
        public static readonly Positions3X3 BottomRightCorner = new Positions3X3 { Row = 2, Col = 2 };

        // Edges
        public static readonly Positions3X3 BottomEdge = new Positions3X3 { Row = 2, Col = 1 };
        public static readonly Positions3X3 LeftEdge = new Positions3X3 { Row = 1, Col = 0 };
        public static readonly Positions3X3 RightEdge = new Positions3X3 { Row = 1, Col = 2 };
        public static readonly Positions3X3 TopEdge = new Positions3X3 { Row = 0, Col = 1 };

        // Center
        public static readonly Positions3X3 Center = new Positions3X3 { Row = 1, Col = 1 };

        private Positions3X3()
        {
        }

        public int Row { get; private set; }
        public int Col { get; private set; }
    }
}