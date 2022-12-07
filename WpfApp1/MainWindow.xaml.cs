using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock latestTextBlockClicked;
        bool findingMatch = false;

        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        float ?bestTime = null;
        
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;

            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            float time = (tenthsOfSecondsElapsed / 10F);
            timeTextBlock.Text = time.ToString("0.0s");

            if (matchesFound == 8) { 
                timer.Stop();
                bestTime = bestTime != null
                    ? (time < bestTime ? time : bestTime)
                    : time;
                
                timeTextBlock.Text = timeTextBlock.Text + " - jeszcze raz?";
                bestTimeTextBlock.Text = "Najlepszy czas: " + bestTime.ToString() + "s";
            }
        }

        private void SetUpGame()
        {
            var emojiList = getEmojiList();
            Random random = new Random();

            foreach(TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name == "timeTextBlock" 
                    || textBlock.Name == "bestTimeTextBlock") continue;

                int index = random.Next(emojiList.Count);
                string nextEmoji = emojiList[index];
                textBlock.Text = nextEmoji;
                textBlock.Visibility = Visibility.Visible;
                emojiList.RemoveAt(index);
            }
            timer.Start();
            matchesFound = 0;
            tenthsOfSecondsElapsed = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (!findingMatch)
            {
                textBlock.Visibility = Visibility.Hidden;
                latestTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == latestTextBlockClicked.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
                matchesFound++;
            }
            else
            { 
                latestTextBlockClicked.Visibility= Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8) 
            {
                SetUpGame();
            }
        }

        private List<string> getEmojiList()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🦝","🦝",
                "🐮","🐮",
                "🐷","🐷",
                "🐗","🐗",
                "🐭","🐭",
                "🐰","🐰",
                "🐻","🐻",
                "🐴","🐴"
            };
            List<string> humanEmoji = new List<string>()
            {
                "👩","👩",
                "👨","👨",
                "👵","👵",
                "👲","👲",
                "🧔","🧔",
                "🎅","🎅",
                "👮‍♀️","👮‍♀️",
                "🕵️‍♀️","🕵️‍♀️",
            };
            List<string> fruitsEmoji = new List<string>()
            {
                "🥝","🥝",
                "🥥","🥥",
                "🍇","🍇",
                "🍍","🍍",
                "🍌","🍌",
                "🍉","🍉",
                "🌶","🌶",
                "🍄","🍄",
            };
            List<string> transportEmoji = new List<string>()
            {
                "🚗","🚗",
                "🛺","🛺",
                "🚖","🚖",
                "🏍","🏍",
                "🛴","🛴",
                "🛩","🛩",
                "🛸", "🛸",
                "🚁","🚁",
            };
            List<string> otherEmoji = new List<string>()
            {
                "🌄", "🌄",
                "⛲", "⛲",
                "♨",  "♨",
                "🛎", "🛎",
                "🌫", "🌫",
                "🌦", "🌦",
                "🌜",  "🌜",
                "☂",  "☂",
            };

            Random random = new Random();
            List<List<string>> list = new List<List<string>>() { animalEmoji, humanEmoji, fruitsEmoji, transportEmoji, otherEmoji};

            return list[random.Next(list.Count)];
        }
    }
}
