using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M1_POO
{
    /**
     * Interface representing a carnivorous animal.
     */
    interface ICarnivore
    {
        void Hunt();
    }

    /**
     * Interface representing a herbivorous animal.
     */
    interface IHerbivore
    {
        void Graze();
    }

    /**
     * Abstract class representing an animal.
     * 
     * Exemple d'abstraction : Animal est une classe abstraite qui ne peut pas être instanciée directement
     * Elle définit des propriétés et méthodes communes à tous les animaux
     * Héritage : Les classes Lion et Elephant héritent de cette classe
     * Polymorphisme : Les méthodes abstraites (Speak) sont redéfinies dans les classes dérivées
     */
    public abstract class Animal
    {
        public abstract string Name { get; }
        public abstract DateTime BirthDate { get; }
        public abstract double Weight { get; }
        public abstract DateTime DateOfArrival { get; }

        public abstract void Speak();

        public int GetAge()
        {
            return (DateTime.Now - BirthDate).Days / 365;
        }

    }

    /**
     * Class representing a lion, inheriting from Animal and implementing ICarnivore
     * 
     * Interface : Implémente ICarnivore pour ajouter le comportement Hunt.
     */
    public class Lion : Animal, ICarnivore
    {
        public override string Name { get; } = "Lion";
        public override DateTime BirthDate { get; }
        public override double Weight { get; }
        public override DateTime DateOfArrival { get; }
        public Lion(DateTime birthDate, double weight, DateTime dateOfArrival)
        {
            BirthDate = birthDate;
            Weight = weight;
            DateOfArrival = dateOfArrival;
        }
        public override void Speak()
        {
            Console.WriteLine("Roar!");
        }

        public void Hunt()
        {
            Console.WriteLine("The lion is hunting.");
        }
    }

    /**
     * Class representing an elephant, inheriting from Animal and implementing IHerbivore.
     * 
     * Interface : Implémente IHerbivore pour ajouter le comportement Graze.
     */
    public class Elephant : Animal, IHerbivore
    {
        public override string Name { get; } = "Elephant";
        public override DateTime BirthDate { get; }
        public override double Weight { get; }
        public override DateTime DateOfArrival { get; }
        public Elephant(DateTime birthDate, double weight, DateTime dateOfArrival)
        {
            BirthDate = birthDate;
            Weight = weight;
            DateOfArrival = dateOfArrival;
        }
        public override void Speak()
        {
            Console.WriteLine("Trumpet!");
        }

        public void Graze()
        {
            Console.WriteLine("The elephant is grazing.");
        }
    }
}
