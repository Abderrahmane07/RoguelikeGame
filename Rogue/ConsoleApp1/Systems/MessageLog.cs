using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace ConsoleApp1.Systems
{
    // Represents a queue of messages that can be added to
    // Has a method for and drawing to an RLConsole
    public class MessageLog
    {
        // Definition du nombre maximal de ligne a conserver
        private static readonly int _maxLines = 9;

        // On utilise la queue pour conserver les textes utilises
        // On applique le principe FIFO (First In First Out)
        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        // Ajout une ligne a la queue de MessageLog
        public void Add(string message)
        {
            _lines.Enqueue(message);

            // Quand on depasse le nombre maximal de lignes on supprime la plus ancienne 
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        // Ajouter chaque ligne dans la queue a la console
        public void Draw(RLConsole console)
        {
            string[] lines = _lines.ToArray();
            for ( int i=0; i<lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}
