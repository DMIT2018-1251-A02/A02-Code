<Query Kind="Statements" />

int[] numberA = [1,2,3,4];
int[] numberB = [3,4,5,6];

numberA.Union(numberB).Dump();
numberA.Intersect(numberB).Dump();
numberA.Except(numberB).Dump();
numberA.Concat(numberB).Dump();