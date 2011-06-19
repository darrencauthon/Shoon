Feature: Denormalize events into a SQL Table
	In order to easily turn events into view
	As a Simple CQRS developer
	I want to use a denormalizer that automates much of the denormalizing work for me

Scenario: Create a new object in the table
	Given the product view model table is empty
	When a product is created with the following data
	| Field           | Value                                |
	| AggregateRootId | 09887969-1F86-47A0-BB47-57722D2DF892 |
	| Sku             | testsku                              |
	Then the following product view models should exist in the Product table
	| AggregateRootId                      | Sku     |
	| 09887969-1F86-47A0-BB47-57722D2DF892 | testsku |

Scenario: Create and update
	Given the product view model table is empty
	When a product is created with the following data
	| Field           | Value                                |
	| AggregateRootId | 4B4FCB75-BC7C-459F-AC1B-EFDA8C0CBFBE |
	| Sku             | SKU #1                               |
	And the name of the product '4B4FCB75-BC7C-459F-AC1B-EFDA8C0CBFBE' is set to 'Applesauce Cleaner'
	Then the following product view models should exist in the Product table
	| AggregateRootId                      | Sku     | Name               |
	| 4B4FCB75-BC7C-459F-AC1B-EFDA8C0CBFBE | SKU #1  | Applesauce Cleaner |

Scenario: Upsert when no previous record exists
	Given the product view model table is empty
	When an event to set the price of product '27BB4FC0-5058-42A2-A97A-0F9027C9F0EB' to 4 without a create event
	Then the following product view models should exist in the Product table
	| AggregateRootId                      | Sku | Name | Price |
	| 27BB4FC0-5058-42A2-A97A-0F9027C9F0EB |     |      | 4     |