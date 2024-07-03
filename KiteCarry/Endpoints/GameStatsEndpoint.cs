namespace KiteCarry.Endpoints
{
    internal class GameStatsEndpoint
    {
        public static string Url => "https://127.0.0.1:2999/liveclientdata/gamestats";
        public double gameTime { get; set; }

        /// <summary>
        /// Calculates and returns the game time in seconds.
        /// </summary>
        public double GetGameTime() => gameTime * 1000;
    }
}
