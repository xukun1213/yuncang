#nullable disable

namespace Huayu.Oms.Domain.AgregatesModel.TenantAggregate;

public class Address : ValueObject
{
    public string Province { get; private set; }
    public string City { get; private set; }
    public string District { get; private set; }
    public string Street { get; private set; }
    public string ZipCode { get; private set; }



    public Address() { }

    public Address(string province, string city, string district, string street, string zipCode)
    {

        Province = province;
        City = city;
        District = district;
        Street = street;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Province;
        yield return City;
        yield return District;
        yield return Street;
        yield return ZipCode;
    }
}

