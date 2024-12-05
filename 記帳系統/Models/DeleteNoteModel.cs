using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Models
{
    public class DeleteNoteModel
    {
        public string NoteDate { get; set; }

        public DeleteNoteModel() { }

        public DeleteNoteModel(string noteDate)
        {
            NoteDate = noteDate;
        }
    }
}
