<Query Kind="Statements" />

List<int> numbers = [1, 2, 3, 4, 5];
var upToThree = numbers.TakeWhile(n => n <= 3);
upToThree.Dump();
