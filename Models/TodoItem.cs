using System.ComponentModel.DataAnnotations;
namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; } // Id laukas, kuris bus naudojamas kaip primary key
        [Required] 
        public string Name { get; set; } = string.Empty; // Name laukas, kuris bus naudojamas kaip uzduoties pavadinimas
        public bool IsComplete { get; set; } // Bool, kuris rodo ar uzduotis yra atlikta
    }
}