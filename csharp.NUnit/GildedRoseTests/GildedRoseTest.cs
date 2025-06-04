using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GildedRoseTests;

public class GildedRoseTest
{
    private IList<Item> _items;
    private GildedRose _app;

    [SetUp]
    public void Setup()
    {
        _items = new List<Item>
        {
            new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
            new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
            new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
            new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
            new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80},
            new Item
            {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = 15,
                Quality = 20
            },
            new Item
            {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = 10,
                Quality = 49
            },
            new Item
            {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = 5,
                Quality = 49
            },
            // this conjured item does not work properly yet
            new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
        };

        _app = new GildedRose(_items);
    }

    [Test]
    public void Foo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("foo"));
    }

    [Test]
    public void ShouldInitializeItemsCorrectly()
    {
        var expectedItems = new List<(string Name, int SellIn, int Quality)>
        {
            ("+5 Dexterity Vest", 10, 20),
            ("Aged Brie", 2, 0),
            ("Elixir of the Mongoose", 5, 7),
            ("Sulfuras, Hand of Ragnaros", 0, 80),
            ("Sulfuras, Hand of Ragnaros", -1, 80),
            ("Backstage passes to a TAFKAL80ETC concert", 15, 20),
            ("Backstage passes to a TAFKAL80ETC concert", 10, 49),
            ("Backstage passes to a TAFKAL80ETC concert", 5, 49),
            ("Conjured Mana Cake", 3, 6)
        };

        for (var i = 0; i < _items.Count; i++)
        {
            Assert.That(_items[i].Name, Is.EqualTo(expectedItems[i].Name));
            Assert.That(_items[i].SellIn, Is.EqualTo(expectedItems[i].SellIn));
            Assert.That(_items[i].Quality, Is.EqualTo(expectedItems[i].Quality));
        }
    }

    [Test]
    public void QualityShouldNeverExceedFifty()
    {
        // Arrange: Set Bries quality to 49
        var brie = _items[1];
        brie.Quality = 49;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(brie.Quality, Is.LessThanOrEqualTo(50));

        // Act again to verify it won't go over 50
        _app.UpdateQuality();

        // Assert again
        Assert.That(brie.Quality, Is.EqualTo(50));
    }

    [Test]
    public void QualityShouldNeverBeNegative()
    {
        // Arrange: Set the Dexterity Vests quality to 1
        var vest = _items[0];
        vest.Quality = 1;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(vest.Quality, Is.GreaterThanOrEqualTo(0));

        // Act again to verify it won't go below 0
        _app.UpdateQuality();

        // Assert again
        Assert.That(vest.Quality, Is.EqualTo(0));
    }

    [Test]
    public void SulfurasQuality_ShouldAlwaysBeEighty()
    {
        // Arrange: Sulfuras items
        var sulfuras1 = _items[3];
        var sulfuras2 = _items[4];

        // Act

        _app.UpdateQuality();
        // Assert

        Assert.That(sulfuras1.Quality, Is.EqualTo(80));
        Assert.That(sulfuras2.Quality, Is.EqualTo(80));
    }

    [Test]
    public void VestOfDexterityQuality_ShouldReduceByOneADay()
    {
        // Arrange: Get the Dexterity Vest
        var vest = _items[0];

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be reduced by 1
        Assert.That(vest.Quality, Is.EqualTo(19));

        // Act again to verify it reduces by 1 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(vest.Quality, Is.EqualTo(18));
    }

    [Test]
    public void ElixirOfTheMongooseQuality_ShouldReduceByOneADay()
    {
        // Arrange: Get the Elixir of the Mongoose
        var elixir = _items[2];

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be reduced by 1
        Assert.That(elixir.Quality, Is.EqualTo(6));
    }

    [Test]
    public void AgedBrieQuality_ShouldIncreaseByOneADay()
    {
        // Arrange: Get the Aged Brie
        var brie = _items[1];
        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be increased by 1
        Assert.That(brie.Quality, Is.EqualTo(1));

        // Act again to verify it increases by 1 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(brie.Quality, Is.EqualTo(2));
    }

    [Test]
    public void VestOfDexterityAndElixirOfTheMongooseQuality_ShouldDegradeTwiceAsFastOnceSellDateHasPassed()
    {
        // Arrange
        var vest = _items[0];
        var elixir = _items[2];

        // Set Sell dates to today
        vest.SellIn = 0;
        elixir.SellIn = 0;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(vest.Quality, Is.EqualTo(18)); // Reduced by 2
        Assert.That(elixir.Quality, Is.EqualTo(5)); // Reduced by 2
    }

    [Test]




}