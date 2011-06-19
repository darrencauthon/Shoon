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

Scenario: Upsert when the record has not been inserted
	Given the product view model table is empty
	When an event to set the price of product '27BB4FC0-5058-42A2-A97A-0F9027C9F0EB' to 4 without a create event
	Then the following product view models should exist in the Product table
	| AggregateRootId                      | Sku | Name | Price |
	| 27BB4FC0-5058-42A2-A97A-0F9027C9F0EB |     |      | 4     |

Scenario: Upsert when the record has already been inserted
	Given the product view model table has the following data
	| AggregateRootId                      | Price |
	| 27BB4FC0-5058-42A2-A97A-0F9027C9F0EB |       |
	When an event to set the price of product '27BB4FC0-5058-42A2-A97A-0F9027C9F0EB' to 4 without a create event
	Then the following product view models should exist in the Product table
	| AggregateRootId                      | Sku | Name | Price |
	| 27BB4FC0-5058-42A2-A97A-0F9027C9F0EB |     |      | 4     |

Scenario: Delete
	Given the product view model table has the following data
	| AggregateRootId                      |
	| 7B1FEAF8-A190-452A-9827-FA615607CDBE |
	When a product is created with the following data
	| Field           | Value                                |
	| AggregateRootId | 27BB4FC0-5058-42A2-A97A-0F9027C9F0EB |
	And an event with id of '27BB4FC0-5058-42A2-A97A-0F9027C9F0EB' is marked as inactive
	Then the following product view models should exist in the Product table
	| AggregateRootId                      |
	| 7B1FEAF8-A190-452A-9827-FA615607CDBE |
