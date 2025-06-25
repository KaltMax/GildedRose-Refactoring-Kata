using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    private readonly IList<Item> _items;
    private const int MaxQuality = 50;
    private const int MinQuality = 0;
    private const int SulfurasQuality = 80;

    private enum ItemType
    {
        AgedBrie,
        BackstagePass,
        Conjured,
        Sulfuras,
        Standard
    }

    public GildedRose(IList<Item> items)
    {
        _items = items;
    }

    public void UpdateQuality()
    {
        foreach (var item in _items)
        {
            var itemType = GetItemType(item);
            UpdateItemQuality(item, itemType);
            UpdateItemSellIn(item, itemType);
            HandleExpiredItem(item, itemType);
        }
    }

    private ItemType GetItemType(Item item)
    {
        if (IsAgedBrie(item)) return ItemType.AgedBrie;
        if (IsBackstagePass(item)) return ItemType.BackstagePass;
        if (IsConjured(item)) return ItemType.Conjured;
        if (IsSulfuras(item)) return ItemType.Sulfuras;
        return ItemType.Standard;
    }

    private void UpdateItemQuality(Item item, ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.AgedBrie:
                IncreaseQuality(item, 1);
                break;
            case ItemType.BackstagePass:
                UpdateBackstagePassesQuality(item);
                break;
            case ItemType.Conjured:
                DecreaseQuality(item, 2);
                break;
            case ItemType.Sulfuras:
                item.Quality = SulfurasQuality;
                break;
            case ItemType.Standard:
                DecreaseQuality(item, 1);
                break;
        }
    }

    private void UpdateBackstagePassesQuality(Item item)
    {
        IncreaseQuality(item, 1);
        if (item.SellIn < 11)
        {
            IncreaseQuality(item, 1);
        }

        if (item.SellIn < 6)
        {
            IncreaseQuality(item, 1);
        }
    }

    private void UpdateItemSellIn(Item item, ItemType itemType)
    {
        if (itemType != ItemType.Sulfuras)
        {
            item.SellIn -= 1;
        }
    }

    private void HandleExpiredItem(Item item, ItemType itemType)
    {
        if (item.SellIn >= 0)
        {
            return;
        }

        switch (itemType)
        {
            case ItemType.AgedBrie:
                IncreaseQuality(item, 1);
                break;
            case ItemType.BackstagePass:
                item.Quality = 0;
                break;
            case ItemType.Conjured:
                DecreaseQuality(item, 2);
                break;
            case ItemType.Sulfuras:
                // Sulfuras does not change
                break;
            case ItemType.Standard:
                DecreaseQuality(item, 1);
                break;
        }
    }

    private void IncreaseQuality(Item item, int amount)
    {
        item.Quality += amount;
        if (item.Quality > MaxQuality)
        {
            item.Quality = MaxQuality;
        }
    }

    private void DecreaseQuality(Item item, int amount)
    {
        if (item.Quality > MinQuality)
        {
            item.Quality -= amount;
            if (item.Quality < MinQuality)
            {
                item.Quality = MinQuality;
            }
        }
    }

    // Helper methods to check item types
    private bool IsAgedBrie(Item item) => item.Name == "Aged Brie";
    private bool IsBackstagePass(Item item) => item.Name.ToLower().Contains("backstage passes");
    private bool IsConjured(Item item) => item.Name.ToLower().Contains("conjured");
    private bool IsSulfuras(Item item) => item.Name.ToLower().Contains("sulfuras");
}