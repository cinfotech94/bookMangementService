﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementService.Domain.DTO.Request
{
    public class CartDTO
    {
        public string username { get; set; }
        public Guid bookId { get; set; }
    }
}
