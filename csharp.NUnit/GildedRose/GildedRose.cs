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
        switch (item.Name)
        {
            case "Aged Brie":
                IncreaseQuality(item);
                break;
            case "Backstage passes to a TAFKAL80ETC concert":
                UpdateBackstagePassesQuality(item);
                break;
            case "Conjured Mana Cake":
                DecreaseQuality(item, 2);
                break;
            case "Sulfuras, Hand of Ragnaros":
                // Sulfuras does not need to be updated
                break;
            default:
                DecreaseQuality(item, 1);
                break;
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
            item.SellIn -= 1;
        }
    }

    private void HandleExpiredItem(Item item)
    {
        if (item.SellIn >= 0)
        {
            return;
        }

        switch (item.Name)
        {
            case "Aged Brie":
                IncreaseQuality(item);
                break;
            case "Backstage passes to a TAFKAL80ETC concert":
                item.Quality = 0;
                break;
            case "Conjured Mana Cake":
                DecreaseQuality(item, 2);
                break;
            case "Sulfuras, Hand of Ragnaros":
                // Sulfuras does not change
                break;
            default:
                DecreaseQuality(item, 1);
                break;
        }
    }

    private void IncreaseQuality(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality += 1;
        }
    }

    private void DecreaseQuality(Item item, int amount)
    {
        if (item.Quality > 0)
        {
            item.Quality -= amount;
            if (item.Quality < 0)
            {
                item.Quality = 0;
            }
        }
    }
}