using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using KiteCarry.Client;
using KiteCarry.Endpoints;
using KiteCarry.Ui;
using WindowsInput;
using WindowsInput.Native;

namespace KiteCarry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InputSimulator _sim = new InputSimulator();
        private readonly LiveClientDataApi _client;

        private bool _isRunning = false;
        private double _lastAaTick;
        private double _lastMoveTick;

        public MainWindow()
        {
            InitializeComponent();

            _client = new LiveClientDataApi();

            ComboBoxHelper.SetComboBoxKeyBinds(
                comboBoxAutoKiteKey,
                comboBoxAttackMoveClickKey,
                comboBoxMoveClickKey
            );

            GetCurrentLeaguePatch();
            UpdateGame();
            CheckLeagueProcess();
        }

        /// <summary>
        /// Get the current League of Legends Patch version and update the UI label
        /// </summary>
        private async void GetCurrentLeaguePatch()
        {
            List<string> versions = await _client.GetEndpointData<List<string>>(
                VersionsEndpoint.Url
            );

            labelCurrentPatch.Content =
                versions != null && versions.Any() ? $"Patch: {versions[0]}" : "Patch: unknown";
        }

        /// <summary>
        /// Continuously checks if the League of Legends process is running and updates the UI label
        /// </summary>
        private async void CheckLeagueProcess()
        {
            while (true)
            {
                _isRunning = Process.GetProcessesByName("League of Legends").Any();
                leagueIsRunning.Text = _isRunning.ToString();
                leagueIsRunning.Foreground = new SolidColorBrush(
                    _isRunning ? Colors.LightGreen : Colors.PaleVioletRed
                );
                await Task.Delay(500);
            }
        }

        /// <summary>
        /// Main Game Update loop contains the auto kiting logic.
        /// </summary>
        private async void UpdateGame()
        {
            while (true)
            {
                if (
                    _isRunning
                    && _sim.InputDeviceState.IsKeyDown(
                        (VirtualKeyCode)comboBoxAutoKiteKey.SelectedItem
                    )
                )
                {
                    ActivePlayerEndpoint activePlayer =
                        await _client.GetEndpointData<ActivePlayerEndpoint>(
                            ActivePlayerEndpoint.Url
                        );

                    GameStatsEndpoint gameStats = await _client.GetEndpointData<GameStatsEndpoint>(
                        GameStatsEndpoint.Url
                    );

                    if (activePlayer != null && gameStats != null)
                    {
                        activePlayer.sliderWindUpValue = sliderWindUp.Value;
                        activePlayer.sliderMoveTimeValue = sliderMoveTime.Value;
                        double clickDelay = sliderClickDelay.Value;

                        double gameTime = gameStats.GetGameTime();
                        double windupTime = activePlayer.GetWindupTime();
                        double moveTime = activePlayer.GetMoveTime();

                        // Press Attack Move Click if the time since the last attack is greater than the sum of windup time and move time
                        if (gameTime - _lastAaTick > windupTime + moveTime)
                        {
                            _sim.Keyboard.KeyPress(
                                (VirtualKeyCode)comboBoxAttackMoveClickKey.SelectedItem
                            );
                            _lastAaTick = gameTime;
                        }
                        // Press Move Click if the time since the last attack is greater than the windup time
                        // and the time since the last move is greater than the click delay
                        else if (
                            gameTime - _lastAaTick > windupTime
                            && gameTime - _lastMoveTick > clickDelay
                        )
                        {
                            _sim.Keyboard.KeyPress(
                                (VirtualKeyCode)comboBoxMoveClickKey.SelectedItem
                            );
                            _lastMoveTick = gameTime;
                        }
                    }
                }
                else
                {
                    _lastAaTick = 0;
                    _lastMoveTick = 0;
                }

                await Task.Delay(16);
            }
        }
    }
}
