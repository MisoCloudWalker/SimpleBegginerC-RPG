using System; // Подключаем основные команды C#.
using System.Threading.Tasks; // Подключаем команды для работы с "ожиданием" (асинхронностью).
using Telegram.Bot.Types; // Подключаем команды для работы с Telegram.

namespace RPG // Та же "папка" для кода, что и в Program.cs.
{
    /// <summary>
    /// Этот класс управляет всей игрой. Он как "мозг" игры.
    /// Может работать в двух режимах: через консоль (экран компьютера) и через Telegram.
    /// </summary>
    public class GameController
    {
        // Это переменные, которые хранят состояние игры (как здоровье и атака).
        private int playerHealth = 100;    // Сколько здоровья у игрока (на старте 100).
        private int playerAttack = 10;     // Сколько урона наносит игрок (всегда 10).
        private int enemyAttack = 10;      // Сколько урона наносит враг (всегда 10).
        private int enemyHealth = 50;      // Сколько здоровья у врага (на старте 50).
        private bool shieldActive = false; // Включена ли защита (true — да, false — нет).
        private bool gameActive = false;   // Идёт ли игра в Telegram (true — да, false — нет).

        // --- Игра в консоли ---

        /// <summary>
        /// Запускает игру в консоли — это основной режим игры.
        /// </summary>
        public void PlayGame()
        {
            // Сбрасываем игру в начальное состояние.
            playerHealth = 100; // У игрока снова 100 здоровья.
            enemyHealth = 50;   // У врага снова 50 здоровья.
            shieldActive = false; // Защита выключена.

            // Это цикл — он повторяет действия, пока здоровье игрока больше 0.
            while (playerHealth > 0)
            {
                UpdateConsole(); // Очищаем экран, чтобы всё выглядело аккуратно.

                // Показываем игроку, что происходит в игре.
                Console.WriteLine($"Твоё здоровье: {playerHealth} | Атака: {playerAttack}");
                Console.WriteLine($"Здоровье врага: {enemyHealth}");
                Console.WriteLine("Выбери действие: 1. Атаковать | 2. Вылечиться | 3. Защититься");

                // Считываем, что выбрал игрок (1, 2 или 3).
                string? input = Console.ReadLine(); // Может быть null, если что-то пошло не так.

                // Если ничего не ввели (null), пропускаем ход.
                if (input == null)
                {
                    Console.WriteLine("Ты ничего не выбрал, ход пропущен.");
                    continue; // Это слово говорит: "начни цикл сначала".
                }

                // Проверяем, что выбрал игрок, и делаем действие.
                if (input == "1") // Если выбрали "1" — атакуем врага.
                    AttackConsole();
                else if (input == "2") // Если выбрали "2" — лечимся.
                    RecoverConsole();
                else if (input == "3") // Если выбрали "3" — включаем защиту.
                    ShieldConsole();
                else // Если ввели что-то неправильное.
                    Console.WriteLine("Ты ввёл что-то не то, попробуй ещё раз.");

                // Проверяем, победили ли мы врага.
                if (enemyHealth <= 0)
                {
                    Console.WriteLine("Ты победил! Поздравляю!");
                    return; // Это слово говорит: "закончи метод и выйди".
                }

                // Теперь враг атакует игрока.
                EnemyAttackConsole();
            }

            // Если здоровье игрока стало 0 или меньше, игра заканчивается.
            Console.WriteLine("Ты погиб, игра окончена.");
        }

        /// <summary>
        /// Очищает экран консоли, чтобы всё выглядело чисто перед новым ходом.
        /// </summary>
        private void UpdateConsole()
        {
            Console.Clear(); // Просто стирает всё с экрана.
        }

        /// <summary>
        /// Игрок атакует врага в консольной игре.
        /// </summary>
        private void AttackConsole()
        {
            enemyHealth -= playerAttack; // Уменьшаем здоровье врага на силу атаки игрока (10).
            Console.WriteLine($"Ты атаковал врага на {playerAttack} урона");
        }

        /// <summary>
        /// Игрок лечит себя в консольной игре.
        /// </summary>
        private void RecoverConsole()
        {
            playerHealth += 20; // Прибавляем 20 к здоровью игрока.
            Console.WriteLine($"Ты восстановил 20 здоровья, теперь у тебя {playerHealth} здоровья");
        }

        /// <summary>
        /// Игрок включает защиту в консольной игре.
        /// </summary>
        private void ShieldConsole()
        {
            shieldActive = true; // Включаем защиту (следующая атака врага будет слабее).
            Console.WriteLine("Ты защитился! Следующая атака врага будет ослаблена.");
        }

        /// <summary>
        /// Враг атакует игрока в консольной игре.
        /// </summary>
        private void EnemyAttackConsole()
        {
            int damage = enemyAttack; // Урон врага начинается с 10.
            if (shieldActive) // Если защита включена.
            {
                damage /= 2; // Делим урон на 2 (становится 5).
                shieldActive = false; // Выключаем защиту после использования.
            }
            playerHealth -= damage; // Уменьшаем здоровье игрока на урон врага.
            Console.WriteLine($"Враг атаковал тебя на {damage} урона, теперь у тебя {playerHealth} здоровья");
        }

        // --- Игра в Telegram ---

        /// <summary>
        /// Запускает игру в Telegram и показывает начальное состояние.
        /// </summary>
        public async Task StartTelegramGame(ChatId chatId) // Изменили на async Task, чтобы ждать отправку сообщения.
        {
            gameActive = true; // Говорим, что игра началась.
            playerHealth = 100; // Сбрасываем здоровье игрока.
            enemyHealth = 50;   // Сбрасываем здоровье врага.
            shieldActive = false; // Выключаем защиту.
            await SendGameState(chatId); // Ждём, пока отправится сообщение в Telegram.
        }

        /// <summary>
        /// Игрок атакует врага в Telegram.
        /// </summary>
        public async Task Attack(ChatId chatId) // "async Task" значит, что метод работает с ожиданием.
        {
            if (!gameActive) // Если игра не начата.
            {
                await GameChatIntegration.SendMessage(chatId, "Игра не начата. Используйте /start.");
                return; // Выходим из метода.
            }

            enemyHealth -= playerAttack; // Уменьшаем здоровье врага.
            await GameChatIntegration.SendMessage(chatId, $"Ты атаковал врага на {playerAttack} урона");

            if (enemyHealth <= 0) // Если враг побеждён.
            {
                await GameChatIntegration.SendMessage(chatId, "Ты победил! Поздравляю!");
                gameActive = false; // Игра закончена.
                return;
            }

            await EnemyAttack(chatId); // Враг атакует в ответ.
            if (playerHealth <= 0) // Если игрок погиб.
            {
                await GameChatIntegration.SendMessage(chatId, "Ты погиб, игра окончена.");
                gameActive = false;
                return;
            }

            await SendGameState(chatId); // Показываем, что теперь в игре.
        }

        /// <summary>
        /// Игрок лечит себя в Telegram.
        /// </summary>
        public async Task Recover(ChatId chatId)
        {
            if (!gameActive) // Если игра не начата.
            {
                await GameChatIntegration.SendMessage(chatId, "Игра не начата. Используйте /start.");
                return;
            }

            playerHealth += 20; // Лечим игрока на 20.
            await GameChatIntegration.SendMessage(chatId, $"Ты восстановил 20 здоровья, теперь у тебя {playerHealth} здоровья");

            await EnemyAttack(chatId); // Враг атакует.
            if (playerHealth <= 0) // Если игрок погиб.
            {
                await GameChatIntegration.SendMessage(chatId, "Ты погиб, игра окончена.");
                gameActive = false;
                return;
            }

            await SendGameState(chatId); // Показываем новое состояние.
        }

        /// <summary>
        /// Игрок включает защиту в Telegram.
        /// </summary>
        public async Task Shield(ChatId chatId)
        {
            if (!gameActive) // Если игра не начата.
            {
                await GameChatIntegration.SendMessage(chatId, "Игра не начата. Используйте /start.");
                return;
            }

            shieldActive = true; // Включаем защиту.
            await GameChatIntegration.SendMessage(chatId, "Ты защитился! Следующая атака врага будет ослаблена.");

            await EnemyAttack(chatId); // Враг атакует.
            if (playerHealth <= 0) // Если игрок погиб.
            {
                await GameChatIntegration.SendMessage(chatId, "Ты погиб, игра окончена.");
                gameActive = false;
                return;
            }

            await SendGameState(chatId); // Показываем новое состояние.
        }

        /// <summary>
        /// Враг атакует игрока в Telegram.
        /// </summary>
        private async Task EnemyAttack(ChatId chatId)
        {
            int damage = enemyAttack; // Урон врага (10).
            if (shieldActive) // Если защита включена.
            {
                damage /= 2; // Уменьшаем урон вдвое (до 5).
                shieldActive = false; // Выключаем защиту.
            }
            playerHealth -= damage; // Уменьшаем здоровье игрока.
            await GameChatIntegration.SendMessage(chatId, $"Враг атаковал тебя на {damage} урона, теперь у тебя {playerHealth} здоровья");
        }

        /// <summary>
        /// Показывает текущее состояние игры в Telegram.
        /// </summary>
        private async Task SendGameState(ChatId chatId)
        {
            // Создаём сообщение с текущим состоянием игры.
            string state = $"Твоё здоровье: {playerHealth} | Атака: {playerAttack}\n" +
                           $"Здоровье врага: {enemyHealth}\n" +
                           "Выбери действие: /1 - Атаковать | /2 - Вылечиться | /3 - Защититься";
            // Отправляем сообщение в Telegram и ждём, пока оно дойдёт.
            await GameChatIntegration.SendMessage(chatId, state);
        }
    }
}