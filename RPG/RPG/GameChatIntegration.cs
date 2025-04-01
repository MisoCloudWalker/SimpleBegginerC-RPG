using System; // Это как коробка с базовыми инструментами для работы с текстом и числами.
using System.Threading; // Это помогает программе "ждать", если что-то делается долго.
using System.Threading.Tasks; // Это для задач, которые не делаются сразу, например, отправка сообщений.
using Telegram.Bot; // Это специальная коробка с инструментами для работы с ботами в Telegram.
using Telegram.Bot.Polling; // Это часть коробки, которая помогает боту "слушать" новые сообщения.
using Telegram.Bot.Types; // Здесь лежат штуки, которые Telegram использует, например, сообщения или чаты.
using Telegram.Bot.Types.Enums; // Это списки вариантов, например, какие бывают сообщения (текст, фото и т.д.).

namespace RPG // Это как папка, где мы храним наш код, чтобы он не потерялся.
{
    /// <summary>
    /// Этот класс — как мостик между игрой и Telegram.
    /// Он берёт команды, которые ты пишешь боту, и отвечает тебе в чате.
    /// </summary>
    public class GameChatIntegration
    {
        // Это наш "телефон" для связи с Telegram. Через него мы будем отправлять сообщения.
        // "null!" значит, что мы пока не настроили телефон, но скоро это сделаем.
        private static TelegramBotClient botClient = null!;

        // Это секретный ключ (как пароль) для нашего бота. Его дал BotFather в Telegram.
        // "readonly" значит, что этот ключ нельзя поменять после того, как мы его написали.
        // Замени YOU_KEY_HEAR на ключ из BotFather
        private static readonly string telegramToken = "YOU_KEY_HEAR";

        // Это наш "управляющий игрой". Он знает всё про игру: здоровье, атаки и т.д.
        // Мы создаём его один раз и используем для всех команд.
        private static GameController gameController = new GameController();

        /// <summary>
        /// Этот метод включает бота, чтобы он начал слушать твои сообщения в Telegram.
        /// </summary>
        public static void Initialize()
        {
            // Создаём "телефон" для Telegram и даём ему наш секретный ключ.
            botClient = new TelegramBotClient(telegramToken);

            // Это как кнопка "выключить", чтобы мы могли остановить бота, если захотим.
            var cts = new CancellationTokenSource();

            // Включаем бота, чтобы он начал слушать, что ты пишешь.
            // Мы говорим ему: "Если придёт сообщение — делай HandleUpdateAsync, если ошибка — делай HandleErrorAsync".
            botClient.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync), // Это как инструкции: что делать с сообщениями и ошибками.
                cancellationToken: cts.Token // Это кнопка "выключить", которую мы сделали выше.
            );
        }

        /// <summary>
        /// Этот метод проверяет, что ты написал боту, и решает, что ему делать.
        /// </summary>
        /// <param name="bot">Наш "телефон" для общения с Telegram.</param>
        /// <param name="update">Это то, что пришло от Telegram: сообщение или что-то ещё.</param>
        /// <param name="cancellationToken">Это кнопка "выключить", чтобы остановить всё, если нужно.</param>
        private static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            // Проверяем, что это сообщение с текстом (не картинка или стикер).
            // "update.Type" — это как тип письма, а "update.Message?.Text" — это сам текст, если он есть.
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
            {
                // Берём текст, который ты написал, и делаем все буквы маленькими.
                // Например, "/START" станет "/start", чтобы проще было проверять.
                string message = update.Message.Text.ToLower();

                // Узнаём адрес чата (номер), чтобы знать, куда отправлять ответ.
                ChatId chatId = update.Message.Chat.Id; // Это как адрес твоего дома в Telegram.

                // Смотрим, что ты написал, и решаем, что делать.
                if (message.Contains("/start")) // Если ты написал "/start".
                {
                    // Отправляем тебе сообщение "Игра началась в Telegram!".
                    // "await" значит "жди, пока сообщение дойдёт".
                    await bot.SendMessage(chatId, "Игра началась в Telegram!");

                    // Запускаем игру и ждём, пока она начнётся (например, отправит первое сообщение).
                    await gameController.StartTelegramGame(chatId);
                }
                else if (message.Contains("/атака") || message == "/1") // Если ты написал "/атака" или "/1".
                {
                    // Говорим игре: "Игрок хочет атаковать врага".
                    // Ждём, пока атака произойдёт и бот ответит.
                    await gameController.Attack(chatId);
                }
                else if (message.Contains("/восстановление") || message == "/2") // Если ты написал "/восстановление" или "/2".
                {
                    // Говорим игре: "Игрок хочет вылечиться".
                    // Ждём, пока лечение произойдёт и бот ответит.
                    await gameController.Recover(chatId);
                }
                else if (message.Contains("/защита") || message == "/3") // Если ты написал "/защита" или "/3".
                {
                    // Говорим игре: "Игрок хочет защититься".
                    // Ждём, пока защита включится и бот ответит.
                    await gameController.Shield(chatId);
                }
                else // Если ты написал что-то непонятное.
                {
                    // Отправляем тебе сообщение, что команда неправильная, и подсказываем правильные.
                    await bot.SendMessage(chatId, "Неизвестная команда. Используйте /start, /атака (/1), /защита (/3), /восстановление (/2)");
                }
            }
        }

        /// <summary>
        /// Этот метод включается, если что-то сломалось в работе с Telegram.
        /// </summary>
        /// <param name="bot">Наш "телефон" для общения с Telegram.</param>
        /// <param name="exception">Это ошибка — как письмо с объяснением, что пошло не так.</param>
        /// <param name="cancellationToken">Это кнопка "выключить", чтобы остановить всё, если нужно.</param>
        private static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            // Пишем в консоль (на экран компьютера), что сломалось, чтобы мы могли это увидеть.
            Console.WriteLine($"Ошибка: {exception.Message}");
            // Говорим программе, что мы закончили обрабатывать ошибку.
            return Task.CompletedTask;
        }

        /// <summary>
        /// Этот метод отправляет сообщение в чат Telegram.
        /// </summary>
        /// <param name="chatId">Это адрес чата, куда отправляем сообщение.</param>
        /// <param name="message">Это текст, который мы хотим отправить.</param>
        public static async Task SendMessage(ChatId chatId, string message)
        {
            // Используем наш "телефон", чтобы отправить сообщение в чат.
            // "await" значит "жди, пока сообщение дойдёт до Telegram".
            await botClient.SendMessage(chatId, message);
        }
    }
}