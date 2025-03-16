using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using System.Collections.Generic;



class Program
{
    private static readonly TelegramBotClient botClient = new TelegramBotClient("7528058827:AAF8uaUentzGSGUCa_DvjEuwal6ew7tbUd0");

    // ذخیره سوالات و پاسخ‌ها
    private static List<string> questions = new List<string>()
    {
        "آیا تجربه کاری دارید؟ (بله/خیر)",
        "چند سال سابقه کار دارید؟",
        "آیا توانایی کار با نرم‌افزارهای فروشگاهی دارید؟ (بله/خیر)"
    };

    private static Dictionary<long, List<string>> userResponses = new Dictionary<long, List<string>>();

    static async Task Main(string[] args)
    {
        Console.WriteLine("ربات در حال اجرا است...");
        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);

        Console.ReadLine();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, System.Threading.CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text != null)
        {
            var chatId = update.Message.Chat.Id;

            // بررسی آیا کاربر پاسخ‌ها را قبلاً شروع کرده است
            if (!userResponses.ContainsKey(chatId))
            {
                userResponses[chatId] = new List<string>();
                await botClient.SendTextMessageAsync(chatId, questions[0]); // ارسال اولین سوال
                return;
            }

            // دریافت پاسخ کاربر
            var responses = userResponses[chatId];
            responses.Add(update.Message.Text);

            // ارسال سوال بعدی
            if (responses.Count < questions.Count)
            {
                await botClient.SendTextMessageAsync(chatId, questions[responses.Count]);
            }
            else
            {
                // نمایش پاسخ‌های کاربر
                await botClient.SendTextMessageAsync(chatId, "پاسخ‌های شما ثبت شد. تحلیل داده‌ها در حال انجام است:");
                foreach (var response in responses)
                {
                    await botClient.SendTextMessageAsync(chatId, response);
                }

                // پاک کردن اطلاعات کاربر (اختیاری)
                userResponses.Remove(chatId);
            }
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
    {
        Console.WriteLine($"خطا: {exception.Message}");
        return Task.CompletedTask;
    }
}
