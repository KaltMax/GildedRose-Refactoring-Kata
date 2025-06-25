using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            UpdateItemQuality(item);
            UpdateItemSellIn(item);
            HandleExpiredItem(item);
        }
    }

    private void UpdateItemQuality(Item item)
    {
        if (item.Name == "Aged Brie")
        {
            IncreaseQuality(item);
        }
        else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            UpdateBackstagePassesQuality(item);
        }
        else if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            DecreaseQuality(item, 1);
        }
    }

    private void UpdateBackstagePassesQuality(Item item)
    {
        IncreaseQuality(item);
        if (item.SellIn < 11)
        {
            IncreaseQuality(item);
        }
        if (item.SellIn < 6)
        {
            IncreaseQuality(item);
        }
    }

    private void UpdateItemSellIn(Item item)
    {
        if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            item.SellIn = item.SellIn - 1;
        }
    }

    private void HandleExpiredItem(Item item)
    {
        if (item.SellIn >= 0)
        {
            return;
        }

        if (item.Name == "Aged Brie")
        {
            IncreaseQuality(item);
        }
        else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            item.Quality = 0;
        }
        else if (item.Name != "Sulfuras, Hand of Ragnaros")
        {
            DecreaseQuality(item, 1);
        }
    }

    private void IncreaseQuality(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality = item.Quality + 1;
        }
    }

    private void DecreaseQuality(Item item, int amount)
    {
        if (item.Quality > 0)
        {
            item.Quality = item.Quality - amount;
            if (item.Quality < 0)
            {
                item.Quality = 0;
            }
        }
    }
}