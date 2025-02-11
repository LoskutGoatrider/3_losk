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
                /// <summary>
                /// метод предназначен для проверки валидности объекта Employee с использованием атрибутов валидации данных
                /// </summary>
                /// <param name="employee">Объект класса Employee, который требуется проверить на валидность.</param>
                /// <returns></returns>
                public string ValidateEmployee(Employee employee)
                {
                        var errorMessages = new List<string>();
                        var validationResults = new List<ValidationResult>();
                        var validationContext = new ValidationContext(employee, null, null);
                        bool isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);
                        if (!isValid)
                        {
                                
                                errorMessages.AddRange(validationResults.Select(vr => vr.ErrorMessage));
                        }
                        return string.Join("\n", errorMessages);
                }
        }
}
