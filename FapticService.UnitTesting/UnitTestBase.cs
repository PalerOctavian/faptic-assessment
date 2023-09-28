using AutoFixture;
using AutoFixture.AutoMoq;

namespace FapticService.UnitTesting;

public class UnitTestBase<TSut> where TSut : class
{
    private Lazy<TSut> LazySut { get; }

    protected TSut Sut => LazySut.Value;
    
    protected IFixture Fixture { get; }
    
    
    protected UnitTestBase()
    {
        LazySut = new Lazy<TSut>(CreateSut);
        Fixture = new Fixture();
        Fixture.Customize(new AutoMoqCustomization());
    }
    
    protected virtual TSut CreateSut() => this.Fixture.Create<TSut>();
}
