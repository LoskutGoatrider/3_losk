using losk_3.BasaSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace losk_3.Services
{
        internal class ValidateEmployees
        {
                public string ValidateEmployee(Employee employee)
                {
                        // Создаем список для хранения сообщений об ошибках валидации
                        var errorMessages = new List<string>();
                        // Создаем список для хранения результатов валидации
                        var validationResults = new List<ValidationResult>();
                        // Создаем контекст для валидации, который содержит проверяемый объект (сотрудника)
                        var validationContext = new ValidationContext(employee, null, null);
                        // Выполняем валидацию объекта сотрудника
                        bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);
                        // Если валидация не прошла успешно
                        if (!isValid)
                        {
                                // Добавляем сообщения об ошибках в список
                                errorMessages.AddRange(validationResults.Select(vr => vr.ErrorMessage));
                        }
                        // Объединяем все сообщения об ошибках в одну строку, разделяя их символом новой строки
                        return string.Join("\n", errorMessages);
                }
        }
}
