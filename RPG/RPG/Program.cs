using System;
class Program
{
    /// <summary>
    /// Тут статистика игрока
    /// </summary>
    static int playerHealth = 100;
    static int playerAttack = 10;

    /// <summary>
    /// Тут статистика противника
    /// </summary>
    static int enemyAttack = 10;
    static int enemyHealth = 50;

    /// <summary>
    /// Старт
    /// </summary>
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в игру");
        PlayGame();
    }


    /// <summary>
    /// Основной цикл игры
    /// </summary>
    static void PlayGame()
    {
        while (playerHealth > 0)
        {
            UpdateConsole();

            Console.WriteLine($"Твоё здоровье {playerHealth} | Атака {playerAttack} ");

            Console.WriteLine($"Здоровье врага {enemyHealth}");

            Console.WriteLine($"Выбери действие: 1. Атаковать | 2. Вылечиться | 3. Защититься");

            string input = Console.ReadLine();

            if (input == "1")
                Attack();
            else if (input == "2")
                Recover();
            else if (input == "3")
                Shield();
            else
                Console.WriteLine("Некорректный ввод");
            if (enemyHealth <= 0)
                EndGame();
            else
                EnemyAttack();
        }
        Console.WriteLine("Ты погиб, игра окончена");
    }

    /// <summary>
    /// Метод, который очищает экран (стирает прошлые действия в строке)
    /// </summary>
    static void UpdateConsole()
    {
        Console.SetCursorPosition(0, 1);
    }

    /// <summary>
    /// Атака по врагу
    /// </summary>
    static void Attack()
    {
        enemyHealth -= playerAttack;
        Console.WriteLine($"Ты атаковал врага на {playerAttack} урона");
    }


    /// <summary>
    /// Вылечиться на определённое значение
    /// </summary>
    static void Recover()
    {
        playerHealth += 20;
        Console.WriteLine($"Ты восстановил 20 здоровья, теперь у тебя {playerHealth} здоровья");
    }


    /// <summary>
    /// Уменьшение урона по игроку
    /// </summary>
    static void Shield()
    {
        enemyAttack /= 2;
        Console.WriteLine($"Ты защитился, у противника уменьшился урон! И теперь он наносит {enemyAttack} урона");
    }


    /// <summary>
    /// Враг атакует игрока
    /// </summary>
    static void EnemyAttack()
    {
        playerHealth -= enemyAttack;
        Console.WriteLine($"Враг атаковал тебя на {enemyAttack} и теперь у тебя {playerHealth} здоровья");
    }


    /// <summary>
    /// Реализация продвижения игры (прокачка персонажа) | нужно понять как сделать новый энкаунтер с противником + добавить каких-нибудь диалогов
    /// </summary>
    static void LevelUp()
    {
        Console.WriteLine("Ты победил, твой уровень повышен и урон стал больше, но не только у тебя)");
        playerAttack += 4;
        enemyHealth = 50;
        enemyAttack += 5;
        playerHealth += 5;
    }
    

    /// <summary>
    /// Конец игры
    /// </summary>
    static void EndGame()
    {
        Console.WriteLine("Ты победил! Отправь секретное словосочестание автору и получи приз!");
        Console.WriteLine("Секретное словосочетание: покажи собаку");
    }
}