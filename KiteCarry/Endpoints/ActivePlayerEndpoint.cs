using KiteCarry.Endpoints.ActivePlayerData;

namespace KiteCarry.Endpoints
{
    internal class ActivePlayerEndpoint
    {
        public static string Url => "https://127.0.0.1:2999/liveclientdata/activeplayer";
        public ChampionStats championStats { get; set; }

        public double sliderWindUpValue { get; set; }

        public double sliderMoveTimeValue { get; set; }

        /// <summary>
        /// Calculates and returns the delay in milliseconds between the attacks of a champion
        /// </summary>
        public double GetAttackDelay() => 1000 / championStats.attackSpeed;

        /// <summary>
        /// Calculates and returns the windup time in milliseconds for a champion's attack
        /// </summary>
        public double GetWindupTime() => GetAttackDelay() * (sliderWindUpValue / 100);

        /// <summary>
        /// Calculates and returns the possible time to move in milliseconds after an attack
        /// </summary>
        public double GetMoveTime() => (GetAttackDelay() - GetWindupTime()) * sliderMoveTimeValue;
    }
}
