﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kafic.ViewModels
{
    public class LoadParams 
    {  
        [FromQuery(Name = "jtStartIndex")]
        [Required]
        [Range(0, int.MaxValue)]
        public int StartIndex { get; set; }

        [FromQuery(Name = "jtPageSize")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "jtPageSize mora biti pozitivan broj")]
        public int Rows { get; set; }

        [FromQuery(Name = "jtSorting")]
        public string Sort { get; set; }

        [FromQuery(Name = "filter")]
        public string Filter { get; set; }

        [BindNever]
        public bool Descending => !string.IsNullOrWhiteSpace(Sort) && Sort.EndsWith("DESC", StringComparison.OrdinalIgnoreCase);

        [BindNever]
        public string SortColumn
        {
            get
            {
                string column = null;
                if (!string.IsNullOrWhiteSpace(Sort))
                {
                    int ind = Sort.IndexOf(' ');
                    if (ind != -1)
                    {
                        column = Sort.Substring(0, ind);
                    }
                    else
                    {
                        column = Sort;
                    }
                }
                return column;
            }
        }

    }
}

