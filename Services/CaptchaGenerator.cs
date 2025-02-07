using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace losk_3.Services
{
        internal class CaptchaGenerator
        {
                private static readonly Random random = new Random();
                private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                /// <summary>
                /// Генерирует случайный текст заданной длины для капчи.
                /// </summary>
                /// <param name="length">Длина текста капчи.</param>
                /// <returns>Случайный текст капчи.</returns>
                public static string GenerateCaptchaText(int length)
                {
                        // Проверяем, что длина текста капчи положительная
                        if (length <= 0)
                                throw new ArgumentException("Длина текста капчи должна быть больше нуля.");
                        // Создаем объект StringBuilder для построения текста капчи заданной длины
                        StringBuilder captchaText = new StringBuilder(length);
                        // Генерируем случайные символы для текста капчи
                        for (int i = 0; i < length; i++)
                        {
                                int index = random.Next(Characters.Length);// Получаем случайный индекс из массива символов
                                captchaText.Append(Characters[index]);// Добавляем случайный символ к тексту капчи
                        }

                        return captchaText.ToString();// Возвращаем сгенерированный текст капчи в виде строки
                }

        }
        }
}