using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M1_POO
{
    /**
     * Class representing a zoo that can hold animals.
     * 
     * Composition : Le Zoo possède une collection d'objets Animal.
     * Polymorphisme : Utilise la méthode Speak sur chaque animal, sans connaître son type exact.
     */
    public class Zoo
    {
        private List<Animal> animals = new List<Animal>();
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }
        public void ListAnimals()
        {
            foreach (var animal in animals)
            {
                Console.WriteLine($"{animal.Name}, Age: {animal.GetAge()}, Weight: {animal.Weight}kg, Arrival: {animal.DateOfArrival.ToShortDateString()}");
                animal.Speak();
                if (animal is ICarnivore carnivore)
                {
                    carnivore.Hunt();
                }
                else if (animal is IHerbivore herbivore)
                {
                    herbivore.Graze();
                }
            }
        }
    }
}
