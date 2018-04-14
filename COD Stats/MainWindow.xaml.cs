using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using COD;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WWII_Stats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Platform selectedPlatform = Platform.PS4;

        private async void textBoxGametag_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    labelStatus.Content = "Requesting users stats...";

                    var stats = WWII.GetProfile(selectedPlatform, textBoxGametag.Text);

                    labelStatus.Content = "User found. Returning results...";

                    // Set Stats in Columns
                    statsWinStreak.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.winStreak));
                    statsWins.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.wins));
                    statsLosses.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.losses));
                    statsCurrentWinStreak.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.currentWinStreak));
                    statsMatchesCompleted.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.matchesCompleted));

                    statsKDRatio.Content = Math.Round(stats.mp.lifetime.all.kdRatio, 2);
                    statsKills.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.kills));
                    statsDeaths.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.deaths));
                    statsHeadshots.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.headshots));
                    statsSuicides.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.suicides));

                    statsXP.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.totalXp));
                    statsScore.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.score));
                    statsKillstreak.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.killStreak));
                    statsBestKills.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.bestKills));
                    statsAccuracy.Content = stats.mp.lifetime.all.accuracy + "%";

                    statsUnlockPoins.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.unlockPoints));
                    statsMoney.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.money));
                    statsPrestigeTokens.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.prestigeShopTokens));
                    statsPoints.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.points));
                    statsTimePlayed.Content = SecondsToTime((int)stats.mp.lifetime.all.timePlayed);

                    // Set Win/Loss Percentage Progress
                    var gamesWonLoss = stats.mp.lifetime.all.wins + stats.mp.lifetime.all.losses;
                    statsWins1.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.wins)) + " WINS";
                    statsLosses1.Content = GetFormattedNumber(Convert.ToString(stats.mp.lifetime.all.losses)) + " LOSSES";
                    progressWinLoss.Maximum = gamesWonLoss;
                    progressWinLoss.Value = stats.mp.lifetime.all.wins;
                    statsWinsPercentage.Content = (int)Math.Round((double)(100 * stats.mp.lifetime.all.wins) / gamesWonLoss) + "%";
                    statsLossesPercentage.Content = (int)Math.Round((double)(100 * stats.mp.lifetime.all.losses) / gamesWonLoss) + "%";

                    // Set Current Level/Prestige Text/Icons
                    statsCurrentLevel.Content = "Level " + stats.mp.level;
                    imageCurrentLevelIcon.Source = GetLevelIcon(stats.mp.level);
                    imageCurrentLevelIcon1.Source = GetLevelIcon(stats.mp.level, 190);

                    // Set Prestige Icon, if user has prestiged
                    if (stats.mp.prestige > 0 && stats.mp.prestige < 9)
                        imageCurrentLevelIcon1.Source = GetPrestigeIcon(stats.mp.prestige);
                    else if (stats.mp.prestige == 10 && stats.mp.level >= 55) // Set master prestige
                        imageCurrentLevelIcon1.Source = GetPrestigeIcon(0, 190, true);

                    // Set Next Level/Prestige Text/Icons
                    if (stats.mp.prestige == 10) // User is at 10th prestige, next is master
                    {
                        if (stats.mp.level < 55) // User is at 10th prestige, but not hit master yet
                        {
                            var nextLevel = stats.mp.level += 1.0;
                            statsNextLevel.Content = "Level " + nextLevel;
                            imageNextLevelIcon.Source = GetLevelIcon(nextLevel);
                        }
                        else if (stats.mp.level == 55) // User is at 10th prestige and max level, next is master
                        {
                            statsNextLevel.Content = "Master Prestige";
                            imageNextLevelIcon.Source = GetPrestigeIcon(0, 70, true);
                        }
                        else if (stats.mp.level < 1000) // User is at master prestige, but not max level yet
                        {
                            var nextLevel = stats.mp.level += 1.0;
                            statsNextLevel.Content = "Level " + nextLevel;
                            imageNextLevelIcon.Source = GetLevelIcon(nextLevel);
                        }
                        else if (stats.mp.level == 1000) // User is at master prestige and hit max level, they've completed it. What now?
                        {
                            statsNextLevel.Content = "Master Prestige";
                            imageNextLevelIcon.Source = GetPrestigeIcon(0, 70, true);
                        }
                    }
                    else
                    {
                        if (stats.mp.level == 55) // User is at max level, show next prestige
                        {
                            var nextPrestige = stats.mp.prestige += 1.0;
                            statsNextLevel.Content = "Prestige " + nextPrestige;
                            imageNextLevelIcon.Source = GetPrestigeIcon(nextPrestige);
                        }
                        else
                        {
                            var nextLevel = stats.mp.level += 1.0;
                            statsNextLevel.Content = "Level " + nextLevel;
                            imageNextLevelIcon.Source = GetLevelIcon(nextLevel);
                        }
                    }

                    // Set Level XP Percentage Progress
                    var levelXPCurrentNeeded = stats.mp.levelXpGained + stats.mp.levelXpRemainder;
                    progressLevelXP.Maximum = levelXPCurrentNeeded;
                    progressLevelXP.Value = stats.mp.levelXpGained;

                    statsLevelXPCurrent.Content = "Current XP: " + GetFormattedNumber(Convert.ToString(stats.mp.levelXpGained));
                    statsLevelXPNeeded.Content = "XP Needed: " + GetFormattedNumber(Convert.ToString(stats.mp.levelXpRemainder));

                    labelUsername.Content = stats.username;
                    labelStatus.Content = $"Successfully returned '{stats.username}' stats.";
                }
                catch (Exception ex)
                {
                    labelStatus.Content = $"Unable to load stats. Error : " + ex.Message;
                    await this.ShowMessageAsync("Error getting users stats", ex.Message, MessageDialogStyle.Affirmative);
                }
            }
        }

        private static BitmapImage GetLevelIcon(double level, int width = 70)
        {
            string levelNumber = level.ToString();

            if (level <= 3)
            {
                levelNumber = "3";
            }
            else if (level <= 6)
            {
                levelNumber = "6";
            }
            else if (level <= 9)
            {
                levelNumber = "9";
            }
            else if (level <= 12)
            {
                levelNumber = "12";
            }
            else if (level <= 15)
            {
                levelNumber = "15";
            }
            else if (level <= 18)
            {
                levelNumber = "18";
            }
            else if (level <= 21)
            {
                levelNumber = "21";
            }
            else if (level <= 24)
            {
                levelNumber = "24";
            }
            else if (level <= 27)
            {
                levelNumber = "27";
            }
            else if (level <= 30)
            {
                levelNumber = "30";
            }
            else if (level <= 33)
            {
                levelNumber = "33";
            }
            else if (level <= 36)
            {
                levelNumber = "36";
            }
            else if (level <= 40)
            {
                levelNumber = "40";
            }
            else if (level <= 41)
            {
                levelNumber = "41";
            }
            else if (level <= 43)
            {
                levelNumber = "43";
            }
            else if (level <= 45)
            {
                levelNumber = "45";
            }
            else if (level <= 47)
            {
                levelNumber = "47";
            }
            else if (level <= 50)
            {
                levelNumber = "50";
            }
            else if (level <= 51)
            {
                levelNumber = "51";
            }
            else if (level <= 52)
            {
                levelNumber = "52";
            }
            else if (level <= 53)
            {
                levelNumber = "53";
            }
            else if (level <= 54)
            {
                levelNumber = "54";
            }
            else if (level <= 55)
            {
                levelNumber = "55";
            }
            else if (level > 55)
            {
                levelNumber = "55";
            }

            return new BitmapImage(new Uri("/Resources/Level/" + levelNumber + ".jpg", UriKind.Relative))
            {
                DecodePixelWidth = width
            };
        }

        private static BitmapImage GetPrestigeIcon(double prestige, int width = 190, bool master = false)
        {
            string prestigeNumber = prestige.ToString();
                        
            if (prestige == 1)
            {
                prestigeNumber = "1";
            }
            else if (prestige == 2)
            {
                prestigeNumber = "2";
            }
            else if (prestige == 3)
            {
                prestigeNumber = "3";
            }
            else if (prestige == 4)
            {
                prestigeNumber = "4";
            }
            else if (prestige == 5)
            {
                prestigeNumber = "5";
            }
            else if (prestige == 6)
            {
                prestigeNumber = "6";
            }
            else if (prestige == 7)
            {
                prestigeNumber = "7";
            }
            else if (prestige == 8)
            {
                prestigeNumber = "8";
            }
            else if (prestige == 9)
            {
                prestigeNumber = "9";
            }
            else if (prestige == 10)
            {
                prestigeNumber = "10";
            }
            else if (master == true)
            {
                prestigeNumber = "master";
            }

            return new BitmapImage(new Uri("/Resources/Prestige/" + prestigeNumber + ".jpg", UriKind.Relative))
            {
                DecodePixelWidth = width
            };
        }

        private void platform_Changed(object sender, RoutedEventArgs e)
        {
            if (radioPSN.IsChecked == true)
            {
                selectedPlatform = Platform.PS4;
            }
            else if (radioXboxLive.IsChecked == true)
            {
                selectedPlatform = Platform.XboxOne;
            }
            else if (radioSteam.IsChecked == true)
            {
                selectedPlatform = Platform.PC;
            }
        }

        /// <summary>
        /// Return Number with Comma's for Thousands
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFormattedNumber(string value)
        {
            return string.Format("{0:n0}", Convert.ToInt32(value));
        }

        public static string SecondsToTime(int value)
        {
            int seconds = value % 60;
            int totalMinutes = value / 60;
            int minutes = totalMinutes % 60;
            int totalHours = totalMinutes / 60;
            int hours = totalHours % 24;
            int totalDays = totalHours / 24;
            StringBuilder builder = new StringBuilder();

            if (totalDays > 0)
            {
                builder.Append(totalDays + " Days ");
            }
            if (totalHours > 0)
            {
                builder.Append(hours + " Hours ");
            }
            if (totalMinutes > 0)
            {
                builder.Append(minutes + " Minutes ");
            }

            builder.Append(seconds + " Seconds");
            return builder.ToString();
        }
    }
}
