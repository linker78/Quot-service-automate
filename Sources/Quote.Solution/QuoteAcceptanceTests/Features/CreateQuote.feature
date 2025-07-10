Feature: CreateQuoteFeature

This feature is about create new quotes in the Quote Service


@CreateQuote
Scenario:  create a new quote with one item for a customer
	Given the following quote details:
        | customer   | item    | quantity | price |discount |
        | John Doe   | Laptop  | 1        | 10.0  | null    |
        | Jane Smith | Tablet  | 2        | 20.0  |	null 	|
    When I create quotes for those customers
    Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: create a new quote with one item with discount for a customer
    Given the following quote details:
        | customer   | item    | quantity | price | discount |
        | John Doe   | Laptop  | 1        | 10.0  | 2.0      |
        | Jane Smith | Tablet  | 2        | 20.0  | 5.0      |
    When I create quotes for those customers
    Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: create a new quote with two items for a customer
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 1        | 10.0  | null     |
		| John Doe   | Mouse   | 2        | 5.0   | null     |
	When I create quotes for those customers
	Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: create a new quote with zero quantity for an item
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 0        | 10.0  | null     |
	When I create quotes for those customers
	Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: create a new quote with maximum quantity for an item
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 1000     | 10.0  | null     |
	When I create quotes for those customers
	Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: create a new quote with negative quantity for an item
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | -1       | 10.0  | null     |
	When I create quotes for those customers
	Then it returns quotes with the correct details and a confirmation message "Quote created successfully."

@CreateQuote
Scenario: Create a new quote with missing customer
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		|            | Laptop  | 1        | 10.0  | null     |
	When I create quotes for those customers
	Then it returns an error message "Customer or Items cannot be null or empty"

@CreateQuote
Scenario: Create a new quote with missing item
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   |  	   | 1        | 10.0  | null     |
	When I create quotes for those customers
	Then it returns an error message "Cannot create the quote for a null item."

@CreateQuote
Scenario: Create a new quote with invalid customer name
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| 12345      | Laptop  | 1        | 10.0  | null     |
	When I create quotes for those customers
	Then it returns an error message "Customer name is invalid."

@CreateQuote
Scenario: Create a new quote with invalid discount
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 1        | 10.0  | -5%     |
	When I create quotes for those customers
	Then it returns a validation error containing "'%' is an invalid end of a number"

@CreateQuote
Scenario: Create a new quote with invalid price
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 1        | -10.0 | null     |
	When I create quotes for those customers
	Then it returns a validation error containing "Price must be greater than or equal to zero."

@CreateQuote
Scenario: Create a new quote with minium quantity and price
	Given the following quote details:
		| customer   | item    | quantity | price | discount |
		| John Doe   | Laptop  | 1        | 0,01   | null     |
	When I create quotes for those customers
	Then it returns quotes with the correct details and a confirmation message "Quote created successfully."