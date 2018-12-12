# rhipe
Rhipe Challenge

Below is the folder structure that's been followed for better maintainability of the code.

	Controllers - All the controllers are placed here.
	ViewModels - View Models used to bind the user inputs and validate them before assigning back to the 
	actual Model.
	Models - Used to send the required data sent to the User.
	Repository - Entire business logic has been placed here and a corresponding interface has been created 
	in order to properly unit test the code.
	Shared - Application level constants and exceptions are defined here.

Business Rule Validations - 

	As per the requirement, validation the input text looking for keywords such as, 
	ISOSCELES/SCALENE/EQUILATERAL triangle, base, height and side measurements.
	Checking the Triangle Inequality rule to decide up on whether a triangle can be drawn based on the 
	given user dimensions.
	Throw exception messages whereever required.
