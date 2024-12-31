using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Domain.DTO.Request;
    public class UserResponseDto
    {

    public Guid Id { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string role { get; set; }
    public string phoneNumber { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string country { get; set; }
    public double balance { get; set; }

}



