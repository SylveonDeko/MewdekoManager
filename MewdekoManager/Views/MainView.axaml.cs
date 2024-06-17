using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media;
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
            BotNameTextBlock = this.FindControl<TextBlock>("BotNameTextBlock");
            BotBannerImage = this.FindControl<Image>("BotBannerImage");
            BotPingTextBlock = this.FindControl<TextBlock>("BotPingTextBlock");
            BotOnlineStatusTextBlock = this.FindControl<TextBlock>("BotOnlineStatusTextBlock");
            BotServerCountTextBlock = this.FindControl<TextBlock>("BotServerCountTextBlock");
            BotHighestMemberServerTextBlock = this.FindControl<TextBlock>("BotHighestMemberServerTextBlock");
            CredentialsTextBlock = this.FindControl<TextBlock>("CredentialsTextBlock");
            MusicTextBlock = this.FindControl<TextBlock>("MusicTextBlock");
            ConfigTextBlock = this.FindControl<TextBlock>("ConfigTextBlock");

            _bots =
            [
                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Name = "Mewdeko Bot 1",
                    Ping = "50ms",
                    OnlineStatus = "Online",
                    ServerCount = "120",
                    HighestMemberServer = "Server A - 5000 members",
                    BannerUrl = "https://cdn.mewdeko.tech/banner.png",
                    Status = "Status for Bot 1",
                    Credentials = "Credentials for Bot 1",
                    Music = "Music for Bot 1",
                    Config = "Config for Bot 1"
                },

                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Name = "Mewdeko Bot 2",
                    Ping = "60ms",
                    OnlineStatus = "Online",
                    ServerCount = "150",
                    HighestMemberServer = "Server B - 6000 members",
                    BannerUrl = "https://cdn.mewdeko.tech/banner.png",
                    Status = "Status for Bot 2",
                    Credentials = "Credentials for Bot 2",
                    Music = "Music for Bot 2",
                    Config = "Config for Bot 2"
                },

                new BotData
                {
                    ImageUrl = "https://cdn.mewdeko.tech/Mewdeko.png",
                    Name = "Mewdeko Bot 3",
                    Ping = "55ms",
                    OnlineStatus = "Online",
                    ServerCount = "130",
                    HighestMemberServer = "Server C - 5500 members",
                    BannerUrl = "https://cdn.mewdeko.tech/banner.png",
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

        private async void OnBotSelected(BotData bot, Image? selectedImage)
        {
            if (ImageStackPanel == null || BotNameTextBlock == null || BotBannerImage == null ||
                BotPingTextBlock == null || BotOnlineStatusTextBlock == null || BotServerCountTextBlock == null ||
                BotHighestMemberServerTextBlock == null || CredentialsTextBlock == null ||
                MusicTextBlock == null || ConfigTextBlock == null || selectedImage == null)
            {
                return;
            }

            foreach (var child in ImageStackPanel.Children)
            {
                if (child is Image img)
                {
                    img.Classes.Remove("selected-glow");
                }
            }

            selectedImage.Classes.Add("selected-glow");

            BotNameTextBlock.Text = bot.Name;
            BotPingTextBlock.Text = $"Ping: {bot.Ping}";
            BotOnlineStatusTextBlock.Text = $"Status: {bot.OnlineStatus}";
            BotServerCountTextBlock.Text = $"Servers: {bot.ServerCount}";
            BotHighestMemberServerTextBlock.Text = $"Top Server: {bot.HighestMemberServer}";

            var bannerBitmap = await LoadImageFromUrlAsync(bot.BannerUrl);
            if (bannerBitmap != null)
            {
                BotBannerImage.Source = bannerBitmap;
            }

            CredentialsTextBlock.Text = bot.Credentials;
            MusicTextBlock.Text = bot.Music;
            ConfigTextBlock.Text = bot.Config;
        }
    }
}