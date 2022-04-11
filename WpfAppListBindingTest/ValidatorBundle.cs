using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppListBindingTest
{
    public class ValidatorBundle
    {
        //public static ValidationResult CustomValidation(string name, ValidationContext context)
        public static ValidationResult CustomValidation(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out var result))
            {
                return ValidationResult.Success;
            }
            return new("IP 주소값의 형식에 맞지 않습니다.");
        }
    }
}
