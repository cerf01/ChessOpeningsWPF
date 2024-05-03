namespace ChessOpeningsWPF.Chess.Movement
{
    public static class Directions
    {
        public static Direction North => new Direction(-1, 0);
        public static Direction South => new Direction(1, 0);
        public static Direction East => new Direction(0, 1);
        public static Direction West => new Direction(0, -1);

        public static Direction NorthEast => North + East;
        public static Direction NorthWest => North + West;
        public static Direction SouthEast => South + East;
        public static Direction SouthWest => South + West;

    }
}
