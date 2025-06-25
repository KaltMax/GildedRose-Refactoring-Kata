using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

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
        vest.SellIn = 0;
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
    public void SulfurasSellIn_ShouldNeverChange()
    {
        // Arrange
        var sulfuras1 = _items[3];
        var sulfuras2 = _items[4];

        // Store initial values
        var initialSellIn1 = sulfuras1.SellIn;
        var initialSellIn2 = sulfuras2.SellIn;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(sulfuras1.SellIn, Is.EqualTo(initialSellIn1));
        Assert.That(sulfuras2.SellIn, Is.EqualTo(initialSellIn2));
    }

    [Test]
    public void VestOfDexterityQuality_ShouldReduceByOneADay()
    {
        // Arrange: Get the Dexterity Vest
        var vest = _items[0];
        var initialQuality = vest.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be reduced by 1
        Assert.That(vest.Quality, Is.EqualTo(initialQuality - 1));

        // Arrange: Store new value before second update
        initialQuality = vest.Quality;

        // Act again to verify it reduces by 1 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(vest.Quality, Is.EqualTo(initialQuality - 1));
    }

    [Test]
    public void VestOfDexterityQuality_ShouldDegradeTwiceAsFastOnceSellDateHasPassed()
    {
        // Arrange
        var vest = _items[0];
        var initialVestQuality = vest.Quality;

        // Set Sell dates to today
        vest.SellIn = 0;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(vest.Quality, Is.EqualTo(initialVestQuality - 2));
    }

    [Test]
    public void VestOfDexteritySellIn_ShouldDecreaseByOneEveryDay()
    {
        // Arrange: Get the Dexterity Vest and store initial SellIn
        var vest = _items[0];
        var initialSellIn = vest.SellIn;

        // Act
        _app.UpdateQuality();

        // Assert: SellIn should be reduced by 1
        Assert.That(vest.SellIn, Is.EqualTo(initialSellIn - 1));
    }

    [Test]
    public void AgedBrieQuality_ShouldIncreaseByOneADay()
    {
        // Arrange: Get the Aged Brie
        var brie = _items[1];
        var initialBrieQuality = brie.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be increased by 1
        Assert.That(brie.Quality, Is.EqualTo(initialBrieQuality + 1));

        // Arrange: Store new value before second update
        initialBrieQuality = brie.Quality;

        // Act again to verify it increases by 1 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(brie.Quality, Is.EqualTo(initialBrieQuality + 1));
    }

    [Test]
    public void AgedBrieQuality_ShouldIncreaseByTwoAfterSellInDate()
    {
        // Arrange: Get the Aged Brie and set SellIn to 0 to simulate the day after sell date
        var brie = _items[1];
        brie.SellIn = 0;
        var initialBrieQuality = brie.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be increased by 2
        Assert.That(brie.Quality, Is.EqualTo(initialBrieQuality + 2));

        // Arrange: Store new value before second update
        initialBrieQuality = brie.Quality;

        // Act again to verify it increases by 2 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(brie.Quality, Is.EqualTo(initialBrieQuality + 2));
    }

    [Test]
    public void AgedBrieSellIn_ShouldDecreaseByOneEveryDay()
    {
        // Arrange: Get the Aged Brie and store initial SellIn
        var brie = _items[1];
        var initialSellIn = brie.SellIn;

        // Act
        _app.UpdateQuality();

        // Assert: SellIn should be reduced by 1
        Assert.That(brie.SellIn, Is.EqualTo(initialSellIn - 1));
    }

    [Test]
    public void ElixirOfTheMongooseQuality_ShouldDegradeTwiceAsFastOnceSellDateHasPassed()
    {
        // Arrange
        var elixir = _items[2];
        var initialElixirQuality = elixir.Quality;

        // Set Sell dates to today
        elixir.SellIn = 0;

        // Act
        _app.UpdateQuality();

        // Assert
        Assert.That(elixir.Quality, Is.EqualTo(initialElixirQuality - 2));
    }

    [Test]
    public void ElixirOfTheMongooseQuality_ShouldReduceByOneADay()
    {
        // Arrange: Get the Elixir of the Mongoose
        var elixir = _items[2];
        var initialElixirQuality = elixir.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be reduced by 1
        Assert.That(elixir.Quality, Is.EqualTo(initialElixirQuality - 1));
    }

    [Test]
    public void ElixirOfTheMongooseSellIn_ShouldDecreaseByOneEveryDay()
    {
        // Arrange: Get the Elixir of the Mongoose and store initial SellIn
        var elixir = _items[2];
        var initialSellIn = elixir.SellIn;

        // Act
        _app.UpdateQuality();

        // Assert: SellIn should be reduced by 1
        Assert.That(elixir.SellIn, Is.EqualTo(initialSellIn - 1));
    }

    [Test]
    public void BackstagePassQuality_ShouldBeZeroAfterConcert()
    {
        // Arrange: Get the Backstage Pass
        var backstagePass = _items[5];

        // Set SellIn to 0 to simulate the concert day
        backstagePass.SellIn = 0;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be 0 after the concert
        Assert.That(backstagePass.Quality, Is.EqualTo(0));
    }

    [Test]
    public void BackstagePassQuality_ShouldIncreaseByOneWhenSellInIsMoreThanTenDays()
    {
        // Arrange: Get the Backstage Pass and set SellIn to more than 10 days
        var backstagePass = _items[5];
        backstagePass.SellIn = 11;
        var initialQualityBackstagePass = backstagePass.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should increase by 1
        Assert.That(backstagePass.Quality, Is.EqualTo(initialQualityBackstagePass + 1));
    }

    [Test]
    public void BackstagePassQuality_ShouldIncreaseByTwoWhenSellInIsLessThanElevenDaysAndMoreThanFiveDays()
    {
        // Arrange: Get the Backstage Pass
        var backstagePass = _items[5];
        backstagePass.SellIn = 10;
        var initialBackstagePassQuality = backstagePass.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert
        Assert.That(backstagePass.Quality, Is.EqualTo(initialBackstagePassQuality + 2));

        // Arrange2: Set SellIn to 6 days and update backstage pass quality
        backstagePass.SellIn = 6;
        initialBackstagePassQuality = backstagePass.Quality;

        // Act2: Update quality for one day
        _app.UpdateQuality();

        // Assert2: Quality should increase by 2
        Assert.That(backstagePass.Quality, Is.EqualTo(initialBackstagePassQuality + 2));
    }

    [Test]
    public void BackstagePassQuality_ShouldIncreaseByThreeWhenSellInIsLessThanSixAndMoreThanZero()
    {
        // Arrange
        var backstagePass = _items[5];
        backstagePass.SellIn = 5;
        var initialQualityBackstagePass = backstagePass.Quality;

        // Act 1
        _app.UpdateQuality();

        // Assert 1
        Assert.That(backstagePass.Quality, Is.EqualTo(initialQualityBackstagePass + 3));

        // Arrange 2: Update initial quality and set SellIn to 1 day
        backstagePass.SellIn = 1;
        initialQualityBackstagePass = backstagePass.Quality;

        // Act 2
        _app.UpdateQuality();

        // Assert
        Assert.That(backstagePass.Quality, Is.EqualTo(initialQualityBackstagePass + 3));
    }

    [Test]
    public void BackstagePassSellIn_ShouldDecreaseByOneEveryDay()
    {
        // Arrange: Get the Backstage Pass and store initial SellIn
        var backstagePass = _items[5];
        var initialSellIn = backstagePass.SellIn;

        // Act
        _app.UpdateQuality();

        // Assert: SellIn should be reduced by 1
        Assert.That(backstagePass.SellIn, Is.EqualTo(initialSellIn - 1));
    }

    [Test]
    public void ConjuredItemsQuality_ShouldDegradeTwiceAsFastAsNormalItems()
    {
        // Arrange: Get the Conjured Mana Cake
        var conjuredItem = _items[8];
        var initialQuality = conjuredItem.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be reduced by 2
        Assert.That(conjuredItem.Quality, Is.EqualTo(initialQuality - 2));

        // Arrange: Store new value before second update
        initialQuality = conjuredItem.Quality;

        // Act again to verify it reduces by 2 each day
        _app.UpdateQuality();

        // Assert again
        Assert.That(conjuredItem.Quality, Is.EqualTo(initialQuality - 2));
    }

    [Test]
    public void ConjuredItemsQuality_ShouldDegradeTwiceAsFastAfterSellInDate()
    {
        // Arrange : Get the Conjured Mana Cake and set SellIn to 0
        var conjuredItem = _items[8];
        conjuredItem.SellIn = 0;
        var initialQuality = conjuredItem.Quality;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert
        Assert.That(conjuredItem.Quality, Is.EqualTo(initialQuality - 4));
    }

    [Test]
    public void ConjuredItemsQuality_ShouldNeverBeUnderZero()
    {
        // Arrange: Get the Conjured Mana Cake and set Quality to 1
        var conjuredItem = _items[8];
        conjuredItem.SellIn = 0; // Set SellIn to 0 to simulate the day after sell date
        conjuredItem.Quality = 1;

        // Act: Update quality for one day
        _app.UpdateQuality();

        // Assert: Quality should be 0 after the update
        Assert.That(conjuredItem.Quality, Is.EqualTo(0));
    }
}