<Query Kind="Statements">
  <Connection>
    <ID>e911c88b-78d8-48b8-9b71-c4628c7c6afb</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

Employees
	.OrderBy(x => x.LastName)
	.Select(x => new
	{
		Name = x.FirstName + " " + x.LastName,
		Dept = x.DepartmentName,
		IncomeCategory = x.BaseRate < 30 ? "Required Review"
										: "No Review Required"
	})
	.ToList()
	.Dump();
	