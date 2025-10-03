using M1_POO;

// Creation of the zoo and adding animals
Zoo zoo = new();
zoo.AddAnimal(new Lion(new DateTime(2018, 5, 1), 190.5, new DateTime(2020, 6, 15)));
zoo.AddAnimal(new Elephant(new DateTime(2015, 3, 12), 5400, new DateTime(2019, 4, 20)));
// List in console all animals in the zoo
zoo.ListAnimals();