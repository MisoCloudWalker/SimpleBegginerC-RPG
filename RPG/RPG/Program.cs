using System;
class Program
{
    static int playerHealth = 100;
    static int playerAttack = 10;
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

            Console.WriteLine($"Выбери действие: 1. Атаковать | 2. Вылечиться ");

            string input = Console.ReadLine();

            if (input == "1")
                Attack();
            else if (input == "2")
                Health();
            else
                Console.WriteLine("Некорректный ввод");
            if (enemyHealth <= 0)
                LevelUp();
            else
                EnemyAttack();
        }
        Console.WriteLine("Ты погиб, игра окончена");
    }

    /// <summary>
    /// Метод, который очищает экран (не оставляет прошлые действия в строке)
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
    static void Health()
    {
        playerHealth += 20;
        Console.WriteLine($"Ты восстановил 20 здоровья, теперь у тебя {playerHealth} здоровья");
    }


    /// <summary>
    /// Враг атакует игрока
    /// </summary>
    static void EnemyAttack()
    {
        Random random = new Random();
        int damage = random.Next(5, 15);
        playerHealth -= damage;
        Console.WriteLine($"Враг атаковал тебя на {damage} и теперь у тебя {playerHealth} здоровья");
    }


    /// <summary>
    /// Реализация продвижения игры (прокачка персонажа)
    /// </summary>
    static void LevelUp()
    {
        Console.WriteLine("Ты победил, твой уровень повышен и урон стал больше, но не только у тебя)");
        playerAttack += 4;
        enemyHealth = 50;
        playerHealth += 5;
    }
}