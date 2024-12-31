using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserAuthManagementService.Domain.DTO.Common;
public class GenericResponse<T>
{
    public bool status { get; set; }
    public string message { get; set; }
    public T data { get; set; }   
}
