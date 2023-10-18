using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GemmaTodoData.Entity
{
    public class TodoItem
    {
        public string Task { get; set; }
        [Key]
        public int Id { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt { get; set; }

        
        

    }
}
