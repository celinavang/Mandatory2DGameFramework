using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.Creatures
{
    public class CreatureObserver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void SubscribeToCreature(Creature creature)
        {
            creature.Died += OnCreatureDied;
        }

        private void OnCreatureDied(object? sender, EventArgs e)
        {
            if (sender is Creature creature)
            {
                Console.WriteLine($"{creature.Name} has died.");
            }
        }
    }
}

