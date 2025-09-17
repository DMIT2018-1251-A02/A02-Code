<Query Kind="Program">
  <Connection>
    <ID>63f7df99-61e5-497e-b9e3-230360397903</ID>
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

void Main()
{
	GetProductCategories().Dump();
}

public List<ProductCategorySummaryView> GetProductCategories()
{
	return ProductCategories
			.Select(pc => new ProductCategorySummaryView
			{
				ProductCategoryName = pc.ProductCategoryName,
				SubCategory = pc.ProductSubcategories
								.Select(psc => new ProductSubcategorySummaryView
								{
									CategoryName = psc.ProductSubcategoryName,
									Description = psc.ProductSubcategoryDescription
								}).OrderBy(x => x.CategoryName)
								.ToList()
			}).OrderBy(x => x.ProductCategoryName)
			.ToList();
}

#region View Models
public class ProductCategorySummaryView
{
	public string ProductCategoryName { get; set; }
	public List<ProductSubcategorySummaryView> SubCategory { get; set; }
}

public class ProductSubcategorySummaryView
{
	public string CategoryName { get; set; }
	public string Description { get; set; }
}
#endregion