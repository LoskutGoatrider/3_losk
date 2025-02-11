using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using losk_3.BasaSQL;

namespace losk_3.Services
{
        internal class Helper          
        {
                /// <summary>
                ///  паттерн проектирования
                /// </summary>
                public static telecom_loskEntities _contex;
                public static telecom_loskEntities GetContext()
                {
                        if (_contex == null)
                        { 
                               _contex = new telecom_loskEntities();
                        }
                        return _contex;
                }
        }
}
