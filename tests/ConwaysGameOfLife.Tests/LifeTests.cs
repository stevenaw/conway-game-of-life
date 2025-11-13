using ConwaysGameOfLife;
using NUnit.Framework;

public class LifeTests
{
    [Test]
    public void Evolve_PreservesDimensions()
    {
        bool[][] data = GetStartingPosition();

        var life = new Life(data);
        life.Evolve();

        Assert.That(life.Width, Is.EqualTo(3));
        Assert.That(life.Height, Is.EqualTo(3));
    }

    [Test]
    public void Evolve_UpdatesWorld()
    {
        bool[][] data = GetStartingPosition();

        var life = new Life(data);

        var gen0 = life.CurrentGeneration;
        life.Evolve();
        var gen1 = life.CurrentGeneration;

        Assert.That(gen0.N, Is.EqualTo(0));
        Assert.That(gen1.N, Is.EqualTo(1));
        Assert.That(gen0.Data, Is.Not.EqualTo(gen1.Data));
    }

    private static bool[][] GetStartingPosition()
    {
        return
        [
            [false, false, false],
            [false, true, false],
            [false, false, false]
        ];
    }
}
