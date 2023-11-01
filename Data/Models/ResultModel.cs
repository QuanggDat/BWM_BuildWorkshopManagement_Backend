using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ResultModel
    {
        public int? Code { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Data { get; set; }
        public bool Succeed { get; set; } = false;
    }
    public class ResponeResultModel
    {
        public int? Code { get; set; }
        public string? ErrorMessage { get; set; }       
    }

    public class PagingModel
    {
        public object? Data { get; set; }
        public int Total { get; set; } = 0;
    }

    public class FileResultModel
    {
        public int? Code { get; set; }
        public string? ErrorMessage { get; set; }
        public byte[]? Data { get; set; }
        public bool Succeed { get; set; } = false;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
