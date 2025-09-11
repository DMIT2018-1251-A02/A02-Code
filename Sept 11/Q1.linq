<Query Kind="Statements" />

int age = 65;

string ageCategory =  age < 13 ? "Child"
					: age < 18 ? "Teen"
					: age < 65 ? "Adult"
					: "Senior";
ageCategory.Dump();