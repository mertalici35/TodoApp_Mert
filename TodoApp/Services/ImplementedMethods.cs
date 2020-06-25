using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class ImplementedMethods : TaskInterface
    {
        public IEnumerable<TodoItem> GetNonOrderedObjectsByReference(int start, TodoContext _context)
        {
            ConcurrentBag<TodoItem> output = new ConcurrentBag<TodoItem>();  // ConcurrentBag is orderless. I used it because it was suggested in the email. 
            var counter = 1;                                                 // To swicth an ordered solution, please  uncommend line 37 and 56,  then comment line 38 and 57 out. 

            foreach (var item in _context.TodoItems)
            {
                if (counter > start)
                {
                    output.Add(item);

                }
                counter++;
                if (counter == start + 6) { break; }
            }
            
            return output.ToList();
        }


        public IList<TodoItem> GetOrderedObjectsByReference(int start, TodoContext _context)
        {
            List<TodoItem> output = new List<TodoItem>();
            var counter = 1;                                                 

            foreach (var item in _context.TodoItems)
            {
                if (counter > start)
                {
                    output.Add(item);

                }
                counter++;
                if (counter == start + 6) { break; }
            }   

            return output;
        }

        
    }
}
