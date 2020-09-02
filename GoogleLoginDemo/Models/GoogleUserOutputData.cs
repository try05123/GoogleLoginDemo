﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleLoginDemo.Models
{
    public class GoogleUserOutputData
    {
        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string email { get; set; }

        public string picture { get; set; }

        public string token { get; set; }
    }
}