Feature:
	login and other test cases automation

@LoginTests
Scenario: Successfull Login
	Given I am on "Live" environment
	And I have opened herokuapp page
	And I am logged in to "herokuapp" with "tomsmith" account
	Then I verify that i am on Logout Page
	Then I Logout from current page
	Then I Close Current Page

@TableTests
######################### TASK 2 IMPLEMENTATION #############################
Scenario: Extract and verify data from the table (Dynamic Table Handling)
	Given I am on "Tables" environment
	And I have opened herokuapp page
	When I extract all company names from the table
	Then I verify if "Jason Doe" exists in the table
	Then I Close Current Page

@U_AlertTests
########################## TASK 3 IMPLEMENTATION #############################
Scenario Outline: Handling different JavaScript alerts
	Given I am on "Alerts" environment
	And I have opened herokuapp page
	When I click on '<alertType>' alert
	Then I handle the '<alertType>' alert with '<action>'
	Then I should see the message '<expectedMessage>'
    
Examples:
	| alertType  | action  | expectedMessage                   |
	| JS Alert   | accept  | You successfully clicked an alert |
	| JS Confirm | accept  | You clicked: Ok                   |
	| JS Confirm | dismiss | You clicked: Cancel               |
	| JS Prompt  | Hello   | You entered: Hello                |


########################## TASK 4 Upload File And File Verification #############################
Scenario: Upload file and verify success
	Given I am on "Upload" environment
	And I have opened herokuapp page
	When I upload a file "testfile"
	Then I should see the uploaded file name "testfile.txt"
