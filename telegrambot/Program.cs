using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBot
{
    class Program
    {
        private static readonly string Token = "7528058827:AAF8uaUentzGSGUCa_DvjEuwal6ew7tbUd0";
        private static readonly TelegramBotClient botClient = new TelegramBotClient(Token);

        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();

            botClient.StartReceiving(
                UpdateHandler,
                ErrorHandler,
                cancellationToken: cts.Token
            );

            Console.WriteLine("🚀 ربات فعال است. برای خروج 'exit' را تایپ کن.");

            while (Console.ReadLine()?.ToLower() != "exit") { }

            cts.Cancel();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { } message && message.Text is not null)
            {
                Console.WriteLine($"📩 پیام جدید از {message.Chat.FirstName}: {message.Text}");
                await bot.SendTextMessageAsync(message.Chat.Id, "✅ پیام شما دریافت شد!", cancellationToken: cancellationToken);
            }
        }

        private static Task ErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"❌ خطای رخ داده: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
