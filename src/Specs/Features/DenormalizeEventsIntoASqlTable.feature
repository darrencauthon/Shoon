Feature: Denormalize events into a SQL Table
	In order to easily turn events into view
	As a Simple CQRS developer
	I want to use a denormalizer that automates much of the denormalizing work for me

Scenario: Create a new object in the table
	Given the product view model table is empty
	When a product is created with the following data
	| Field           | Value                                |
	| AggregateRootId | 09887969-1F86-47A0-BB47-57722D2DF892 |
	Then the following product view models should exist in the Product table
	| AggregateRootId                      |
	| 09887969-1F86-47A0-BB47-57722D2DF892 |