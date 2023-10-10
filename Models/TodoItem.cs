using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Tarefa { get; set; }
        public string Status { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string Observacao { get; set; }
    }
}
