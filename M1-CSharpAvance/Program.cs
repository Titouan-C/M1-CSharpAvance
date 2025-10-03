// See https://aka.ms/new-console-template for more information

// Ecrire en console
Console.WriteLine("Quel est votre calcul ?");

// Lire en console
var calcul = Console.ReadLine();

if (string.IsNullOrWhiteSpace(calcul))
{
    Console.WriteLine("Calcul invalide");
    return;
}

// Définissions des opérateurs
var operators = new[] { '+', '-', '*', '/' };
var operatorFound = operators.FirstOrDefault(op => calcul.Contains(op));
if (operatorFound == default)
{
    Console.WriteLine("Opérateur invalide");
    return;
}

var parts = calcul.Split(operatorFound);
if (parts.Length != 2)
{
    Console.WriteLine("Calcul invalide");
    return;
}
if (!double.TryParse(parts[0], out var left))
{
    Console.WriteLine("Nombre gauche invalide");
    return;
}
if (!double.TryParse(parts[1], out var right))
{
    Console.WriteLine("Nombre droit invalide");
    return;
}
var result = operatorFound switch
{
    '+' => left + right,
    '-' => left - right,
    '*' => left * right,
    '/' => right != 0 ? left / right : double.NaN,
    _ => double.NaN
};
Console.WriteLine($"Résultat: {result}");