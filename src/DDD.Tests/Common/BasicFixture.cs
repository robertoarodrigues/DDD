using Bogus;
using System;

namespace DDD.Tests.Common;
public abstract class BasicFixture
{
    protected BasicFixture() => Faker = new Faker("pt_BR");
    public Faker Faker { get; set; }

}
