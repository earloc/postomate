
Feature: PostmanAutomation

Scenario Outline: A person can be created via postman-automation

	Given the person '<firstName>' '<surname>'
	When the person is posted to the api
	Then the response-statuscode is '<response-statuscode>'

	Examples: 
	| firstName | surname    | response-statuscode |
	| Arthur    | Dent       | 200                 |
	| Zaphod    | Beeblebrox | 200                 |