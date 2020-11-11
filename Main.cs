using System;
namespace Victorina
{
    class VictorinaProgram
    {
        static void Main()
        {
            bool isContinue;
            bool isTicketSelected;
            bool isQuestionResponsed;
            bool isQuestionCleared;
            do
            {
                isContinue = false;
                isTicketSelected = false;
                isQuestionResponsed = false;
                isQuestionCleared = false;
                byte selectedTicket;
                string[,] dataArray =
                {
                    { "Без какой силы мы улетим в небо?", "Лёгкий", "притяжение", "1", "2", "3" },
                    { "Без какой силы мы не сможем остановиться?", "Лёгкий", "трение", "1", "2", "3" },
                    { "При деформации тела, какая сила стремиться вернуть тело в исходное состояние?", "Лёгкий", "упругость", "1", "2", "3" },
                    { "Это сила, с которой тело вследствие притяжения к земле действует на опору или подвес?", "Лёгкий", "вес тела", "1", "2", "3" },
                    { "Это сила, с которой тела притягиваються к земле?", "Лёгкий", "Сила тяжести", "", "", "" },
                    { "Формула силы тяжести", "Средний", "f = m*g", "", "", "" },
                    { "Формула веса тела", "Средний", "p = g*m", "", "", "" },
                    { "Формула центробежной силы", "Средний", "f = m*v^2/r", "", "", "" },
                    { "Формула силы упругости", "Средний", "f = -k*x", "", "", "" },
                    { "Формула силы трения", "Средний", "f = u*n", "", "", "" },
                    { "Вопрос 1?", "Сложный", "Правильный ответ", "", "", "" },
                    { "Вопрос 2?", "Сложный", "Правильный ответ", "", "", "" },
                    { "Вопрос 3?", "Сложный", "Правильный ответ", "", "", "" },
                    { "Вопрос 4?", "Сложный", "Правильный ответ", "", "", "" },
                    { "Вопрос 5?", "Сложный", "Правильный ответ", "", "", "" }
                };
                Random random = new Random();
                int[,] ticketsArray = new int[2, dataArray.GetLength(0)];
                for (int i = 0; i < ticketsArray.GetLength(1); i++)
                {
                    ticketsArray[0, i] = i;
                    ticketsArray[1, i] = 0;
                }
                for (int i = ticketsArray.GetLength(1) - 1; i >= 1; i--)
                {
                    int j = random.Next(i + 1);
                    var temp = ticketsArray[0, j];
                    ticketsArray[0, j] = ticketsArray[0, i];
                    ticketsArray[0, i] = temp;
                }
                do
                {
                    do
                    {
                        selectedTicket = RenderTickets(ticketsArray, dataArray, isQuestionCleared);
                        isTicketSelected = selectedTicket == byte.MaxValue ? false : true;
                    } while (isTicketSelected == false);
                    do
                    {
                        ticketsArray[1, selectedTicket] = RenderQuestion(dataArray, selectedTicket, ticketsArray, random);
                        isQuestionResponsed = ticketsArray[1, selectedTicket] == byte.MaxValue || ticketsArray[1, selectedTicket] == 3 ? false : true;
                    } while (isQuestionResponsed == false);
                    isQuestionCleared = true;
                    for (int i = 1; i < dataArray.GetLength(0); i++)
                    {
                        if (ticketsArray[1, i-1] == 0)
                            isQuestionCleared = false;
                    }
                } while (isQuestionCleared == false);
                RenderTickets(ticketsArray, dataArray, isQuestionCleared);


                isContinue = End();
                Console.Clear();
            } while (isContinue == true);
            Console.Clear();
            Console.WriteLine("\n\t#############\n\t# Викторина #\n\t#############\n\n\tСпасибо за игру.\n");
        }
        static bool End()
        {
            Console.Clear();
            Console.Write("\n\t#############\n\t# Викторина #\n\t#############\n\n\t[1] Продолжить игру?\n\n\tВаш ответ: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    return true;
                default:
                    return false;
            }
        }
        static byte RenderTickets(int[,] ticketsArray, string[,] dataArray, bool isQuestionCleared)
        {
            Console.Clear();
            Console.WriteLine("\n\t##################\n\t# Выберите билет #\n\t##################\n");
            for (int i = 0; i < ticketsArray.GetLength(1); i++)
            {
                Console.WriteLine("\t[{0}] Билет {1}.", i + 1, ticketsArray[1, i] == 1 ? "выполнен успешно" : ticketsArray[1, i] == 2 ? "выполнен неверно" : "не выполнен");
            }
            if (isQuestionCleared == true)
            {
                Console.Write("\n\tИгра окончена, все билеты закрыты!\n\tНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return 0;
            }
            Console.Write("\n\tВаш ответ: ");
            byte userResult;
            if (byte.TryParse(Console.ReadKey().KeyChar.ToString() + Console.ReadKey().KeyChar.ToString(), out userResult) == true)
            {
                if (userResult > dataArray.GetLength(0) || userResult < 1)
                {
                    ErrorMessage("Вы должны ввести число только в указанном диапазоне, попробуйте ещё раз");
                    return byte.MaxValue;
                }
            }
            else
            {
                ErrorMessage("Вы должны ввести число, попробуйте ещё раз");
                return byte.MaxValue;
            }
            if (ticketsArray[1, --userResult] != 0)
            {
                ErrorMessage("Вы пытаетесь выбрать уже выполненную задачу");
                return byte.MaxValue;
            }
            return userResult;
        }
        static byte RenderQuestion(string[,] dataArray, byte selectedTicket, int[,] ticketsArray, Random random)
        {
            Console.Clear();
            Console.Write("\n\t##############\n\t# Ваш вопрос #\n\t##############\n\n\tСложность: {0}.\n\t{1}\n\n", dataArray[ticketsArray[0, selectedTicket], 1], dataArray[ticketsArray[0, selectedTicket], 0]);
            string[,] answersArray = new string[2, 4];
            for (int i = 0; i < 4; i++)
            {
                answersArray[0, i] = i.ToString();
                answersArray[1, i] = dataArray[ticketsArray[0, selectedTicket], 2 + i];
            }
            for (int i = 4 - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = answersArray[0, j];
                answersArray[0, j] = answersArray[0, i];
                answersArray[0, i] = temp;
            }
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("\t[{0}] {1}", i + 1, answersArray[1, int.Parse(answersArray[0, i])]);
            }
            Console.Write("\n\tВаш ответ: ");
            byte userResult;
            if (byte.TryParse(Console.ReadKey().KeyChar.ToString(), out userResult) == true)
            {
                if (userResult > 4 || userResult < 1)
                {
                    ErrorMessage("Вы должны ввести число только в указанном диапазоне, попробуйте ещё раз");
                    return byte.MaxValue;
                }
                if (answersArray[1, int.Parse(answersArray[0, userResult - 1])] == answersArray[1, 0])
                {
                    Console.Write("\n\n\tВы ввели правильный ответ!\n\tНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return 1;
                }
                else if (ticketsArray[1, selectedTicket] == 0)
                {
                    Console.Write("\n\n\tВы ввели не правильный ответ! У вас есть вторая попытка.\n\tНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return 3;
                }
                else
                {
                    Console.Write("\n\n\tВы ввели не правильный ответ!\n\tНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    return 2;
                }
            }
            else
            {
                ErrorMessage("Вы должны ввести число, попробуйте ещё раз");
                return byte.MaxValue;
            }
        }
        static void ErrorMessage(string message)
        {
            Console.Clear();
            Console.Write("\n\t##########\n\t# Ошибка #\n\t##########\n\n\t{0}.\n\tНажмите любую клавишу для продолжения...", message);
            Console.ReadKey();
        }
    }
}
