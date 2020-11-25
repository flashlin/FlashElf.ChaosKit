namespace ChaosSite.Models.Services
{
	public class MyRepo : IMyRepo
	{
		public Customer GetCustomer()
		{
			return new Customer()
			{
				Name = "flash"
			};
		}
	}
}