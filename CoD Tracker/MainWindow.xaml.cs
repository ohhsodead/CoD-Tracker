using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using COD;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CoD_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SelectedPlatform = Platform.PS4;
            selectedLeaderboardsTime = Time.Lifetime;
            selectedLeaderboardsMode = WWII.Gamemode.Career;
            selectedLeaderboardsPage = 0;
            selectedLeaderboardsTotalPages = 0;
            GridOverview.Visibility = Visibility.Visible;
            GridWeekly.Visibility = Visibility.Collapsed;
            GridLeaderboards.Visibility = Visibility.Collapsed;
        }

        public static string GitHubRepo { get; } = "https://github.com/HerbL27/CoD-WWII/";

        public static string selectedUsername { get; set; }
        public static Time selectedLeaderboardsTime { get; set; }
        public static WWII.Gamemode selectedLeaderboardsMode { get; set; }
        public static int selectedLeaderboardsPage { get; set; }
        public static int selectedLeaderboardsTotalPages { get; set; }

        private Platform selectedPlatform;
        public Platform SelectedPlatform
        {
            get { return selectedPlatform; }
            set
            {
                selectedPlatform = value;

                if (selectedPlatform == Platform.PS4)
                {
                    imagePlatformPS4.Source = GetPlatformIcon("playstation", true);
                    imagePlatformXbox.Source = GetPlatformIcon("xbox", false);
                    imagePlatformSteam.Source = GetPlatformIcon("steam", false);

                    imageSelectedPlatform.Source = GetPlatformIcon("playstation", false);
                }
                else if (selectedPlatform == Platform.XboxOne)
                {
                    imagePlatformPS4.Source = GetPlatformIcon("playstation", false);
                    imagePlatformXbox.Source = GetPlatformIcon("xbox", true);
                    imagePlatformSteam.Source = GetPlatformIcon("steam", false);

                    imageSelectedPlatform.Source = GetPlatformIcon("xbox", false);
                }
                else if (selectedPlatform == Platform.PC)
                {
                    imagePlatformPS4.Source = GetPlatformIcon("playstation", false);
                    imagePlatformXbox.Source = GetPlatformIcon("xbox", false);
                    imagePlatformSteam.Source = GetPlatformIcon("steam", true);

                    imageSelectedPlatform.Source = GetPlatformIcon("steam", false);
                }
            }
        }
        
        public static BitmapImage GetPlatformIcon(string platform, bool original)
        {
            string originalText = (original == false ? null : "-original");
            return new BitmapImage(new Uri("/Resources/" + platform + originalText + ".png", UriKind.Relative));
        }

        private void imagePlatform_Changed(object sender, RoutedEventArgs e)
        {
            var Sender = (System.Windows.Controls.Image)sender;

            if (Sender == imagePlatformPS4)
            {
                SelectedPlatform = Platform.PS4;
            }
            else if (Sender == imagePlatformXbox)
            {
                SelectedPlatform = Platform.XboxOne;
            }
            else if (Sender == imagePlatformSteam)
            {
                SelectedPlatform = Platform.PC;
            }
        }

        private async void textBoxUsername_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (textBoxUsername.Text != "")
                {
                    try
                    {
                        marqueeSearchBox.Visibility = Visibility.Visible;

                        selectedUsername = textBoxUsername.Text;
                        labelUsername.Content = selectedUsername;

                        var usersStats = await Task.Run(() => WWII.GetProfile(SelectedPlatform, selectedUsername));

                        // Dislay Users Stats
                        statWinPercent.Content = (int)Math.Round((100 * usersStats.mp.lifetime.all.wins) / (usersStats.mp.lifetime.all.wins + usersStats.mp.lifetime.all.losses)) + "%";
                        statKDRatio.Content = Math.Round(usersStats.mp.lifetime.all.kdRatio, 2);
                        statAccuracy.Content = usersStats.mp.lifetime.all.accuracy + "%";
                        statGameScore.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.score));
                        statTimePlayed.Content = StringUtilities.SecondsToTime((int)usersStats.mp.lifetime.all.timePlayed);

                        // Current Prestige/Level (TODO: add rank text, add prestige text...)
                        // GetLevelRank(usersStats.mp.level);
                        statLevel.Content = "LEVEL " + usersStats.mp.level;
                        imageRank.Source = GetLevelIcon(usersStats.mp.level);

                        // Set Prestige Icon, if user has prestiged
                        if (usersStats.mp.prestige > 0 && usersStats.mp.prestige < 9)
                        {
                            imageRank.Source = GetPrestigeIcon(usersStats.mp.prestige);
                        }
                        else if (usersStats.mp.prestige == 10 && usersStats.mp.level >= 55) // Master Prestige
                        {
                            imageRank.Source = GetPrestigeIcon(0, true);
                            statLevel.Content = "MASTER PRESTIGE";
                        }

                        // Level XP Progress
                        progressLevelXP.Maximum = (usersStats.mp.levelXpGained + usersStats.mp.levelXpRemainder);
                        progressLevelXP.Value = usersStats.mp.levelXpGained;
                        levelCurrentXP.Content = "Current: " + StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.levelXpGained));
                        levelNeededXP.Content = "Needed: " + StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.levelXpRemainder));

                        // Overview Stats
                        statKills.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.kills));
                        statDeaths.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.deaths));
                        statHeadshots.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.headshots));
                        statSuicides.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.suicides));
                        statWins.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.wins));
                        statLosses.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.losses));
                        statWinStreak.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.currentWinStreak));
                        statGamesPlayed.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.matchesPlayed));
                        statTotalXP.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.totalXp));

                        statPlants.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.plants));
                        statDefuses.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.defuses));
                        statConfirmed.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.confirmed));
                        statDenied.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.denied));
                        statCaptures.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.captures));
                        statDefends.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.defends));
                        statDestructions.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.destructions));

                        statBestKills.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.bestKills));
                        statBestKillStreak.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.killStreak));
                        statBestScore.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.bestScore));
                        statBestAccuracy.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.bestAccuracy));
                        statBestWinStreak.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.winStreak));

                        statUnlockPoints.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.unlockPoints));
                        statMoney.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.money));
                        statPrestigeTokens.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.prestigeShopTokens));
                        statPoints.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.lifetime.all.points));

                        // Weekly Stats

                        statWeeklyKills.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.kills));
                        statWeeklyDeaths.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.deaths));
                        statWeeklyAssists.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.assists));
                        statWeeklyKDRatio.Content = Math.Round(usersStats.mp.weekly.all.kdRatio, 2);
                        statWeeklyHeadshots.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.headshots));
                        statWeeklyWins.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.wins));
                        statWeeklyLosses.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.losses));
                        statWeeklyMatchesPlayed.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.matchesPlayed));
                        statWeeklySPM.Content = Math.Round(usersStats.mp.weekly.all.scorePerMinute, 0);
                        statWeeklyNemesisKills.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.nemesisKills));
                        statWeeklyNemesisDeaths.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.nemesisDeaths));
                        statWeeklyTimePlayed.Content = StringUtilities.SecondsToTime((int)usersStats.mp.weekly.all.timePlayed);
                        statWeeklyScore.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.score));
                        statWeeklyTotalXP.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.totalXp));

                        statWeeklyShotsFired.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.shotsFired));
                        statWeeklyShotsLanded.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.shotsLanded));
                        statWeeklyShotsMissed.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.shotsMissed));
                        statWeeklyAccuracy.Content = (int)Math.Round((100 * usersStats.mp.weekly.all.shotsLanded) / (usersStats.mp.weekly.all.shotsLanded + usersStats.mp.weekly.all.shotsMissed)) + "%";

                        statWeeklyDivisionXpInfantry.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.divisionXpInfantry));
                        statWeeklyDivisionXpAirborne.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.divisionXpAirborne));
                        statWeeklyDivisionXpArmored.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.divisionXpArmored));
                        statWeeklyDivisionXpMountain.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.divisionXpMountain));
                        statWeeklyDivisionXpExpeditionary.Content = StringUtilities.GetFormattedNumber(Convert.ToString(usersStats.mp.weekly.all.divisionXpExpeditionary));

                        // Leaderboards
                        var usersLeaderboards = await Task.Run(() => WWII.GetLeaderboards(SelectedPlatform, selectedLeaderboardsTime, selectedLeaderboardsMode, selectedUsername));

                        AddRowsToDataGrid(usersLeaderboards.entries);
                        selectedLeaderboardsPage = usersLeaderboards.page;
                        selectedLeaderboardsTotalPages = usersLeaderboards.totalPages;

                        if (usersLeaderboards.page == 1)
                            buttonLeaderboardsPreviousPage.IsEnabled = false;
                        else
                            buttonLeaderboardsPreviousPage.IsEnabled = true;

                        if (usersLeaderboards.page == usersLeaderboards.totalPages)
                            buttonLeaderboardsNextPage.IsEnabled = false;
                        else
                            buttonLeaderboardsNextPage.IsEnabled = true;

                        marqueeSearchBox.Visibility = Visibility.Hidden;
                    }
                    catch (Exception ex)
                    {
                        marqueeSearchBox.Visibility = Visibility.Hidden;
                        await this.ShowMessageAsync("Oops...", "Unable to retrieve users stats. Make sure the username is valid or try again later." + Environment.NewLine + Environment.NewLine + "Error: " + ex.Message, MessageDialogStyle.Affirmative);
                    }
                }
                else
                {
                    await this.ShowMessageAsync("Oops...", "You must enter a username", MessageDialogStyle.Affirmative);
                }
            }
        }

        /// <summary>
        /// Adds users data to list and binds items to DataGrid
        /// </summary>
        public void AddRowsToDataGrid(List<COD.Models.WWII.Leaderboards.Entry> rows)
        {
            if (DataGridLeaderboards.ItemsSource != null)
            {
                DataGridLeaderboards.ItemsSource = null;
            }

            dataRows.Clear();
            DataGridLeaderboards.Items.Clear();

            foreach (var row in rows)
            {
                dataRows.Add(new GridRow(StringUtilities.GetFormattedNumber(Convert.ToString(row.rank)), row.values.level, row.username, StringUtilities.GetFormattedNumber(Convert.ToString(row.values.score)), Math.Round(row.values.scorePerMinute, 0), row.values.gamesPlayed, StringUtilities.SecondsToTime((int)row.values.timePlayed)));
            }

            DataGridLeaderboards.ItemsSource = dataRows;
        }

        List<GridRow> dataRows = new List<GridRow>();

        public class GridRow
        {
            [DisplayName("Position")]
            public string Position { get; set; }

            [DisplayName("Level")]
            public double Level { get; set; }

            [DisplayName("Name")]
            public string Name { get; set; }

            [DisplayName("Total Score")]
            public string TotalScore { get; set; }

            [DisplayName("SPM")]
            public double SPM { get; set; }

            [DisplayName("Games Played")]
            public double GamesPlayed { get; set; }

            [DisplayName("Time Played")]
            public string TimePlayed { get; set; }

            public GridRow(string position, double level, string name, string totalscore, double spm, double gamesplayed, string timeplayed)
            {
                Position = position;
                Level = level;
                Name = name;
                TotalScore = totalscore;
                SPM = spm;
                GamesPlayed = gamesplayed;
                TimePlayed = timeplayed;
            }
        }

        public static BitmapImage GetLevelIcon(double level)
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
            else if (level <= 39)
            {
                levelNumber = "39";
            }
            else if (level <= 42)
            {
                levelNumber = "42";
            }
            else if (level <= 45)
            {
                levelNumber = "45";
            }
            else if (level <= 48)
            {
                levelNumber = "48";
            }
            else if (level <= 51)
            {
                levelNumber = "51";
            }
            else if (level <= 54)
            {
                levelNumber = "54";
            }
            else if (level > 55)
            {
                levelNumber = "55";
            }

            return new BitmapImage(new Uri("/Resources/Level/" + levelNumber + ".png", UriKind.Relative));
        }

        public static BitmapImage GetPrestigeIcon(double prestige, bool master = false)
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

            return new BitmapImage(new Uri("/Resources/Prestige/" + prestigeNumber + ".png", UriKind.Relative));
        }

        public static string GetLevelRank(double level)
        {
            switch (level)
            {
                case 1:
                    return "Private";
                case 2:
                    return "Private I";
                case 3:
                    return "Private II";
                case 4:
                    return "Private First Class";
                case 5:
                    return "Private First Class I";
                case 6:
                    return "Private First Class II";
                case 7:
                    return "Technician Fifth Grade";
                case 8:
                    return "Technician Fifth Grade I";
                case 9:
                    return "Technician Fifth Grade II";
                case 10:
                    return "Corporal";
                case 11:
                    return "Corporal I";
                case 12:
                    return "Corporal II";
                case 13:
                    return "Technician Fourth Grade";
                case 14:
                    return "Technician Fourth Grade I";
                case 15:
                    return "Technician Fourth Grade II";
                case 16:
                    return "Sergeant";
                case 17:
                    return "Sergeant I";
                case 18:
                    return "Sergeant II";
                case 19:
                    return "Technician Third Grade";
                case 20:
                    return "Technician Third Grade I";
                case 21:
                    return "Technician Third Grade II";
                case 22:
                    return "Staff Sergeant";
                case 23:
                    return "Staff Sergeant I";
                case 24:
                    return "Staff Sergeant II";
                case 25:
                    return "Technical Sergeant";
                case 26:
                    return "Technical Sergeant I";
                case 27:
                    return "Technical Sergeant II";
                case 28:
                    return "First Sergeant";
                case 29:
                    return "First Sergeant I";
                case 30:
                    return "First Sergeant II";
                case 31:
                    return "Master Sergeant";
                case 32:
                    return "Master Sergeant I";
                case 33:
                    return "Master Sergeant II";
                case 34:
                    return "Sergeant Major";
                case 35:
                    return "Sergeant Major I";
                case 36:
                    return "Sergeant Major II";
                case 37:
                    return "Command Sergeant Major";
                case 38:
                    return "Command Sergeant Major I";
                case 39:
                    return "Command Sergeant Major II";
                case 40:
                    return "Second Lieutenant";
                case 41:
                    return "Second Lieutenant I";
                case 42:
                    return "First Lieutenant";
                case 43:
                    return "First Lieutenant I";
                case 44:
                    return "Captain";
                case 45:
                    return "Captain I";
                case 46:
                    return "Major";
                case 47:
                    return "Major I";
                case 48:
                    return "Lieutenant Colonel";
                case 49:
                    return "Lieutenant Colonel II";
                case 50:
                    return "Colonel";
                case 51:
                    return "Brigadier General";
                case 52:
                    return "Major General";
                case 53:
                    return "Lieutenant General";
                case 54:
                    return "General";
                case 55:
                    return "General of the Army";
            }

            // else it's master prestige
            return "MASTER PRESTIGE";
        }

        // Footer / Status Bar
        private void BetaFeedback_Clicked(object sender, MouseEventArgs e)
        {
            Process.Start(GitHubRepo + "issues/new");
        }

        // Profile Tab Headings
        private void TabOverview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridOverview.Visibility = Visibility.Visible;
            GridWeekly.Visibility = Visibility.Collapsed;
            GridLeaderboards.Visibility = Visibility.Collapsed;
        }

        private void TabWeekly_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridOverview.Visibility = Visibility.Collapsed;
            GridWeekly.Visibility = Visibility.Visible;
            GridLeaderboards.Visibility = Visibility.Collapsed;
        }

        private void TabLeaderboards_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridOverview.Visibility = Visibility.Collapsed;
            GridWeekly.Visibility = Visibility.Collapsed;
            GridLeaderboards.Visibility = Visibility.Visible;
        }

        // Leaderboards
        private void comboBoxLeaderboardsTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;

            if (value == "LIFETIME")
            {
                selectedLeaderboardsTime = Time.Lifetime;
            }
            else if (value == "WEEKLY")
            {
                selectedLeaderboardsTime = Time.Weekly;
            }
            else if (value == "MONTHLY")
            {
                selectedLeaderboardsTime = Time.Monthly;
            }
        }

        private void comboBoxLeaderboardsGamemode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;

            if (value == "CAREER")
            {
                selectedLeaderboardsMode = WWII.Gamemode.Career;
            }
            else if (value == "TEAM DEATHMATCH")
            {
                selectedLeaderboardsMode = WWII.Gamemode.TDM;
            }
            else if (value == "FREE FOR ALL")
            {
                selectedLeaderboardsMode = WWII.Gamemode.FreeForAll;
            }
            else if (value == "KILL CONFIRMED")
            {
                selectedLeaderboardsMode = WWII.Gamemode.KillConfirmed;
            }
            else if (value == "CAPTURE THE FLAG")
            {
                selectedLeaderboardsMode = WWII.Gamemode.CaptureTheFlag;
            }
            else if (value == "SEARCH && DESTROY")
            {
                selectedLeaderboardsMode = WWII.Gamemode.SearchAndDestroy;
            }
            else if (value == "DOMINATION")
            {
                selectedLeaderboardsMode = WWII.Gamemode.Domination;
            }
            else if (value == "GRIDIRON")
            {
                selectedLeaderboardsMode = WWII.Gamemode.Gridiron;
            }
            else if (value == "HARDPOINT")
            {
                selectedLeaderboardsMode = WWII.Gamemode.Hardpoint;
            }
            else if (value == "1V1")
            {
                selectedLeaderboardsMode = WWII.Gamemode.OnevOne;
            }
            else if (value == "WAR")
            {
                selectedLeaderboardsMode = WWII.Gamemode.War;
            }
        }

        private async void buttonLeaderboardsPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            selectedLeaderboardsPage -= 1;

            var usersLeaderboards = await Task.Run(() => WWII.GetLeaderboards(SelectedPlatform, selectedLeaderboardsTime, selectedLeaderboardsMode, selectedLeaderboardsPage));

            if (usersLeaderboards.page == 1)
                buttonLeaderboardsPreviousPage.IsEnabled = false;
            else
                buttonLeaderboardsPreviousPage.IsEnabled = true;

            if (usersLeaderboards.page == usersLeaderboards.totalPages)
                buttonLeaderboardsNextPage.IsEnabled = false;
            else
                buttonLeaderboardsNextPage.IsEnabled = true;

            AddRowsToDataGrid(usersLeaderboards.entries);
            selectedLeaderboardsPage = usersLeaderboards.page;
            selectedLeaderboardsTotalPages = usersLeaderboards.totalPages;
        }

        private async void buttonLeaderboardsNextPage_Click(object sender, RoutedEventArgs e)
        {
            selectedLeaderboardsPage += 1;

            var usersLeaderboards = await Task.Run(() => WWII.GetLeaderboards(SelectedPlatform, selectedLeaderboardsTime, selectedLeaderboardsMode, selectedLeaderboardsPage));

            if (usersLeaderboards.page == 1)
                buttonLeaderboardsPreviousPage.IsEnabled = false;
            else
                buttonLeaderboardsPreviousPage.IsEnabled = true;

            if (usersLeaderboards.page == usersLeaderboards.totalPages)
                buttonLeaderboardsNextPage.IsEnabled = false;
            else
                buttonLeaderboardsNextPage.IsEnabled = true;

            AddRowsToDataGrid(usersLeaderboards.entries);
            selectedLeaderboardsPage = usersLeaderboards.page;
            selectedLeaderboardsTotalPages = usersLeaderboards.totalPages;
        }

        private async void ShowLeaderboardsData()
        {
            var usersLeaderboards = await Task.Run(() => WWII.GetLeaderboards(SelectedPlatform, selectedLeaderboardsTime, selectedLeaderboardsMode, selectedLeaderboardsPage));

            if (usersLeaderboards.page == 1)
                buttonLeaderboardsPreviousPage.IsEnabled = false;
            else
                buttonLeaderboardsPreviousPage.IsEnabled = true;

            if (usersLeaderboards.page == usersLeaderboards.totalPages)
                buttonLeaderboardsNextPage.IsEnabled = false;
            else
                buttonLeaderboardsNextPage.IsEnabled = true;

            AddRowsToDataGrid(usersLeaderboards.entries);
            selectedLeaderboardsPage = usersLeaderboards.page;
            selectedLeaderboardsTotalPages = usersLeaderboards.totalPages;
        }

        private void appbar_refresh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowLeaderboardsData();
        }

        private void buttonUpdateStats_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
