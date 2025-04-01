using System;

class Program
{
    /// <summary>
    /// Статистика игрока: здоровье и базовая атака.
    /// </summary>
    static int playerHealth = 100;
    static int playerAttack = 10;

    /// <summary>
    /// Статистика противника: базовый урон и здоровье.
    /// </summary>
    static int enemyAttack = 10;
    static int enemyHealth = 50;

    /// <summary>
    /// Флаг, указывающий, что игрок применил защиту для следующей атаки врага.
    /// </summary>
    static bool shieldActive = false;

    /// <summary>
    /// Точка входа в игру.
    /// </summary>
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в игру");
        PlayGame();
    }

    /// <summary>
    /// Основной игровой цикл: игрок выбирает действие, затем атакует враг.
    /// </summary>
    static void PlayGame()
    {
        while (playerHealth > 0)
        {
            // Выводим текущую статистику
            Console.WriteLine($"Твоё здоровье: {playerHealth} | Атака: {playerAttack}");
            Console.WriteLine($"Здоровье врага: {enemyHealth}");
            Console.WriteLine("Выбери действие: 1. Атаковать | 2. Вылечиться | 3. Защититься");

            string input = Console.ReadLine()!;

            // Обрабатываем выбор игрока
            if (input == "1")
                Attack();
            else if (input == "2")
                Recover();
            else if (input == "3")
                Shield();
            else
                Console.WriteLine("Некорректный ввод");

            // Если враг побежден, завершаем бой
            if (enemyHealth <= 0)
            {
                EndGame();
                return;
            }

            // Ход врага
            EnemyAttack();
        }

        Console.WriteLine("Ты погиб, игра окончена");
    }

    /// <summary>
    /// Метод атаки: уменьшает здоровье противника на величину атаки игрока.
    /// </summary>
    static void Attack()
    {
        enemyHealth -= playerAttack;
        Console.WriteLine($"Ты атаковал врага на {playerAttack} урона");
    }

    /// <summary>
    /// Метод лечения: увеличивает здоровье игрока на 20 единиц.
    /// </summary>
    static void Recover()
    {
        playerHealth += 20;
        Console.WriteLine($"Ты восстановил 20 здоровья, теперь у тебя {playerHealth} здоровья");
    }

    /// <summary>
    /// Метод защиты: снижает урон следующей атаки врага вдвое.
    /// </summary>
    static void Shield()
    {
        shieldActive = true; // Активируем флаг защиты
        Console.WriteLine("Ты защитился! Следующая атака врага будет ослаблена.");
    }

    /// <summary>
    /// Метод атаки противника: если защита активна, урон уменьшается вдвое.
    /// </summary>
    static void EnemyAttack()
    {
        int damage = enemyAttack;

        if (shieldActive)
        {
            damage /= 2; // Временное уменьшение урона
            shieldActive = false; // Сбрасываем флаг защиты
        }

        playerHealth -= damage;
        Console.WriteLine($"Враг атаковал тебя на {damage} урона, теперь у тебя {playerHealth} здоровья");
    }

    /// <summary>
    /// Метод завершения боя: выводит сообщение о победе.
    /// </summary>
    static void EndGame()
    {
        Console.WriteLine("Ты победил! Поздравляю!");
    }
}
