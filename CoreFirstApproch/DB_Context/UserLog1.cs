﻿using System;
using System.Collections.Generic;

#nullable disable

namespace CoreFirstApproch.DB_Context
{
    public partial class UserLog1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Password { get; set; }
    }
}
