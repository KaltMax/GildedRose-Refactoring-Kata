# Gilded Rose starting position in C# NUnit
(Amin Beri, Mohamed Allam, Jennifer Posch, Lukas Kalab, Maximilian Kaltenreiner)

## Build the project

Use your normal build tools to build the projects in Debug mode.
For example, you can use the `dotnet` command line tool:

``` cmd
dotnet build GildedRose.sln -c Debug
```

## Run the Gilded Rose Command-Line program

For e.g. 10 days:

``` cmd
GildedRose/bin/Debug/net8.0/GildedRose 10
```

## Run all the unit tests

``` cmd
dotnet test
```


## Unit Test Strategy

We implemented a test suite that covers all business rules from the Gilded Rose requirements specification. Our test strategy includes:

1. **Single Responsibility Tests**: Each test focuses on verifying one specific rule or behaviour.

2. **Boundary Testing**: We test at the boundaries of the specification (e.g., quality never exceeding 50, quality never negative) to ensure these critical constraints are enforced.

3. **Special Item Handling**: Separate tests for each special item type (Aged Brie, Sulfuras, Backstage passes) to verify their unique behaviors.

4. **Edge Case Coverage**: Testing combinations of boundary conditions such as quality at limits and sell-by dates.

5. **Approval Testing**: Using the VerifyNUnit framework to capture and verify the output of running the application over 30 days, helping detect any regressions in overall system behavior.


## Test Categories

Our unit tests cover these key categories:

1. **General Requirements**:
   - Items correctly initialized with expected values
   - Quality never exceeding 50 (except for Sulfuras)
   - Quality never becoming negative

2. **Normal Items**:
   - Quality decreasing by 1 each day
   - Quality decreasing twice as fast after expiration

3. **Special Items**:
   - **Aged Brie**: Increasing in quality over time, increasing faster after expiration
   - **Sulfuras**: Quality always remaining at 80, SellIn never changing (applies to any item containing "sulfuras" in its name)
   - **Backstage Passes**: Quality increasing by different rates based on time until concert, dropping to 0 after concert (applies to any item containing "backstage passes" in its name)

4. **Conjured Items**:
   - Quality decreasing twice as fast as normal items (applies to any item containing "conjured" in its name)


## Test Structure

We organized our tests using a clear naming convention that describes the expected behavior, making it easy to identify failures and understand what each test is verifying. Most tests follow an Arrange-Act-Assert pattern with descriptive comments to enhance readability.


## Refactoring Decisions

1. **Reading the requirements documentation**: The GildedRoseRequirementsSpecification was crucial for understanding the intended behavior of the system.

2. **Analyzing the Legacy Code**: We carefully examined the original code to identify the core functionalities.

3. **Unit Tests**: Creating unit tests for the existing code helped us understand its behavior and edge cases.

4. **Refactoring Strategy**: We applied the Single Responsibility Principle, breaking down the complex `UpdateQuality` method into smaller, more manageable helper-methods that handle specific item types and behaviours.

5. **Type-Based Item Processing**: Introduced an `ItemType` enum to categorize items more clearly and make the code more maintainable compared to string based switching.

6. **Constant Values**: Added named constants for important values (`MaxQuality`, `MinQuality`, `SulfurasQuality`) to eliminate magic numbers and improve code clarity.

7. **Parameterized Quality Adjustments**: Enhanced quality modification methods to accept amount parameters, making them more versatile and reducing code duplication.

8. **Fixing the Conjured Item feature**: Implemented proper handling of Conjured items with dedicated type recognition and ensuring they degrade in quality twice as fast as normal items.

9. **Flexible item identification**: Replaced exact name matching with partial, case-insensitive string comparison for item categories (Backstage passes, Conjured items, Sulfuras). These changes make it possible to add new items later on without the need for extensive code changes.

10. **Immutability Improvements**: Used `readonly` modifiers for fields to prevent accidental modification and improve design.


## Implementation Structure

The refactored implementation follows a clean separation of concerns:

1. **Item Type Detection**: Centralized logic for determining item types in `GetItemType` method with helper methods for each special type
   
2. **Quality Management**: Dedicated methods to handle quality increases and decreases with proper boundary checking

3. **Processing Flow**: Three-stage processing for each item:
   - `UpdateItemQuality`: Updates quality based on item type
   - `UpdateItemSellIn`: Updates the sell-by date
   - `HandleExpiredItem`: Applies special rules for expired items

4. **Type Flexibility**: String pattern matching rather than exact matches for better extendability.


## Challenges Faced

### Refactoring

1. **Legacy Code Understanding**: The most significant challenge was comprehending what the code actually did without clear documentation. The GildedRoseRequirementSpecification was essential for quickly understanding the intended behavior.

2. **Complex Conditional Logic**: The nested if-statements in the original code made it challenging to identify all edge cases and ensure complete test coverage.

3. **Testing State Changes**: Items undergo different behaviors based on their current state and type, requiring careful test design to verify all possible combinations.

4. **Special Item Rules**: Each special item type (Aged Brie, Sulfuras, Backstage passes) follows different rules with multiple conditions, requiring detailed testing for each scenario.

5. **Conjured Items**: Introducing the new category of conjured items required careful integration with existing logic while ensuring all rules were correctly applied.