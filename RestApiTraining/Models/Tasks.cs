﻿using System;
using System.Collections.Generic;

namespace RestApiTraining.Models
{
    public class Task
    {
        public int id { get; set; }
        public string task { get; set; }
        public bool done { get; set; } = false;
    }
}
