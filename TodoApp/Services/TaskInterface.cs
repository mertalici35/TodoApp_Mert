using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface TaskInterface
    {

        IList<TodoItem> GetOrderedObjectsByReference(int start, TodoContext _context);

        IEnumerable<TodoItem> GetNonOrderedObjectsByReference(int start, TodoContext _context);

    }
}
