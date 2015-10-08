using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Location : ValueObjectBase<Location>
	{
		private Location(){}

		public string City { get; private set; }
		public string Country { get; private set; }

		public Location(string city, string country)
		{
			if (string.IsNullOrEmpty(city)) throw new ArgumentNullException("City");
			if (string.IsNullOrEmpty(country)) throw new ArgumentNullException("Country");
			City = city;
			Country = country;
		}

		public Location WithCity(string city)
		{
			return new Location(city, this.Country);
		}

		public Location WithCountry(string country)
		{
			return new Location(this.City, country);
		}

		public override bool Equals(Location other)
		{
			return this.City.Equals(other.City, StringComparison.InvariantCultureIgnoreCase)
				&& this.Country.Equals(other.Country, StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is Location)) return false;
			return this.Equals((Location)obj);
		}

		public override int GetHashCode()
		{
			return this.City.GetHashCode() + this.Country.GetHashCode();
		}
	}
}
