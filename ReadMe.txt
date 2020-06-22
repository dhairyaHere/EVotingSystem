Title : Poll-Kholl - A service developed by Fruit Salad Technologies Pvt. Ltd.

Description:
	Poll-Kholl is an free, easy to use online voting system. This site facilitates students living out of
	state to contribute in their local elections by casting their precious vote.
	
	Whenever a guest user visits the site, he/she can take a glimpse of all the ongoing as well as 
	completed elections! Although, the guest can not cast their vote until and unless he/she logs into 
	the system.

Users: 
	Common People - Voters ( End User )
	Higher Authority  - Host ( Admin )

Home: 
	Firstly, end user will be directed to the home page and "index.cshtml" view will be rendered.
	
	Description of Index.cshtml :
	This page is basically contains the moto of our web app. Why you should go for such websites and 
	more importantly why choose this website i.e. Poll kholl.
	
Conrollers' Description:

	Following is the brief description of all the controllers used in this web app.
	
	1.) ManagePolls Controller:
	
		Ongoing polls, Completed polls, Details of each polls, its contestants and their details 
		such as symbol,age place etc., actual casting of vote and all such functionalities are implemented
		via this controller.		
			
	2.) Elections Controller:
	
		This controller enables and helps admin to host new elections, edit existing elections' details,
		End an ongoing election and all other higher authorities functionalities.

	3.) Contestants Controller:
	
		Admin can add or modify contestants for a particular election. Admin can also disqualify any contestant
		of an ongoing election if admin finds him a destructive.

	4.) Account Conroller, Manage Controller etc. are automatically created using Identity.
	
Contributers Information:

	*	Dhairya Acharya ( CE SEM 5  - Backend developer )
		Contact Information : 9265324087
		Email : dhairya2308@gmail.com
		Contribution : Admin Side Module
			
			
	*	Darshan Gohel ( CE SEM 5 - Hardcore Graphics Designer )
		Contact Information : 9974700761
		Email : darshan.gohel619@gmail.com
		Contribution : End User Module	