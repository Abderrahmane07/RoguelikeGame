using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Systems
{
    class SchedulingSystem
    {
        private int _time;
        private readonly SortedDictionary<int, List<IScheduleable>> _scheduleables;

        public SchedulingSystem()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<int, List<IScheduleable>>();
        }

        // Ajouter un nouveau objet au schedule et on ajoute au temps actuel +le temps de l'objet
        public void Add(IScheduleable scheduleable)
        {
            int key = _time + scheduleable.Time;
            if (!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<IScheduleable>());
            }
            _scheduleables[key].Add(scheduleable);
        }

        // Enlever un objet du schedule et peut etre utile ppour enlever des monstres morts avant l'arrivee de leur tour
        public void Remove(IScheduleable scheduleable)
        {
            KeyValuePair<int, List<IScheduleable>> scheduleableListFound = new KeyValuePair<int, List<IScheduleable>>(-1, null);

            foreach(var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }
            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if(scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        } 

        // Obtenir l'objet qui a le tour suivant dans la liste 
        public IScheduleable Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }

        // Obtenir le temps actuel
        public int GetTime()
        {
            return _time;
        }
        
        // Rafraichir le temps et vider le schedule
        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }

    }
}
