﻿using System.ComponentModel.DataAnnotations;

namespace Tu_Deuda.Model
{
    public class CodeApp
    {
        [Key]
        public int Id { get; set; }
        public string CodeAdmin { get; set; }
    }
}