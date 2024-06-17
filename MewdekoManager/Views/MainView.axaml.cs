using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia;
using MewdekoManager.Common.Models;

namespace MewdekoManager.Views
{
    public partial class MainView : UserControl
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private List<BotData> _bots;
     

        public MainView()
        {
            InitializeComponent();
            AddImagesAsync();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ImageStackPanel = this.FindControl<StackPanel>("ImageStackPanel");
            StatusTextBlock = this.FindControl<TextBlock>("StatusTextBlock");
            CredentialsTextBlock = this.FindControl<TextBlock>("CredentialsTextBlock");
            MusicTextBlock = this.FindControl<TextBlock>("MusicTextBlock");
            ConfigTextBlock = this.FindControl<TextBlock>("ConfigTextBlock");

            _bots =
            [
                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Status = "Status for Bot 1",
                    Credentials = "Credentials for Bot 1",
                    Music = "Music for Bot 1",
                    Config = "Config for Bot 1"
                },
                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Status = "Status for Bot 2",
                    Credentials = "Credentials for Bot 2",
                    Music = "Music for Bot 2",
                    Config = "Config for Bot 2"
                },
                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Status = "Status for Bot 3",
                    Credentials = "Credentials for Bot 3",
                    Music = "Music for Bot 3",
                    Config = "Config for Bot 3"
                }
            ];
        }

        private static async Task<Bitmap?> LoadImageFromUrlAsync(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync();
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from URL {url}: {ex.Message}");
                return null;
            }
        }

        private async void AddImagesAsync()
        {
            foreach (var bot in _bots)
            {
                var bitmap = await LoadImageFromUrlAsync(bot.ImageUrl);
                if (bitmap == null) continue;
                var image = new Image
                {
                    Source = bitmap,
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(10),
                    Classes = { "rounded", "glow" }
                };

                image.PointerPressed += (sender, e) => OnBotSelected(bot, sender as Image);
                ImageStackPanel.Children.Add(image);
            }
        }

        private void OnBotSelected(BotData bot, Image selectedImage)
        {
            foreach (var child in ImageStackPanel.Children)
            {
                if (child is Image img)
                {
                    img.Classes.Remove("selected-glow");
                }
            }
            
            selectedImage.Classes.Add("selected-glow");
            
            StatusTextBlock.Text = bot.Status;
            CredentialsTextBlock.Text = bot.Credentials;
            MusicTextBlock.Text = bot.Music;
            ConfigTextBlock.Text = bot.Config;
        }
    }
}
