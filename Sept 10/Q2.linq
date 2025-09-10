<Query Kind="Program" />

void Main()
{
	int number1 = 15;
	int number2 = 15;
	ReturnLargestIfElse(number1, number2).Dump();
	ReturnLargestTenary(number1, number2).Dump();
}

public int ReturnLargestIfElse(int num1, int num2)
{
	if (num1 > num2)
	{
		return num1;
	}
	else
		if (num2 > num1)
	{
		return num2;
	}
	else
	{
		return 0;
	}
}

public int ReturnLargestTenary(int num1, int num2)
{
	return num1 > num2 ? num1 
				: num2 > num1 ? num2 
				: 0;
}

