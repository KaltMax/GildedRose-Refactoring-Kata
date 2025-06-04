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

We implemented a comprehensive test suite that covers all business rules from the Gilded Rose requirements specification. Our test strategy includes:

1. **Single Responsibility Tests**: Each test focuses on verifying one specific rule or behavior to ensure clarity and maintainability.

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
   - **Sulfuras**: Quality always remaining at 80, SellIn never changing
   - **Backstage Passes**: Quality increasing by different rates based on time until concert, dropping to 0 after concert


## Test Structure

We organized our tests using a clear naming convention that describes the expected behavior, making it easy to identify failures and understand what each test is verifying. Most tests follow an Arrange-Act-Assert pattern with descriptive comments to enhance readability.


## Challenges Faced

1. **Legacy Code Understanding**: The most significant challenge was comprehending what the code actually did without clear documentation. The GildedRoseRequirementSpecification was essential for understanding the intended behavior.

2. **Complex Conditional Logic**: The nested if-statements in the original code made it challenging to identify all edge cases and ensure complete test coverage.

3. **Testing State Changes**: Items undergo different behaviors based on their current state and type, requiring careful test design to verify all possible combinations.

4. **Special Item Rules**: Each special item type (Aged Brie, Sulfuras, Backstage passes) follows different rules with multiple conditions, requiring detailed testing for each scenario.
