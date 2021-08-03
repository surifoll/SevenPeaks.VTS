# SevenPeaks.VTS


## Required

1. Dotnet Core 5.0
2. Docker

## Tech & Tools

1. Dotnet Core
2. MSSQL
3. RabbitMQ
3. EntityFramework

## Steps to clone and run SevenPeaks
1. Clone the project
1. open terminal/cmd 
2. navigate to the root of the solution folder
3.  run "docker-compose up"
4.  open cmd/terminal 
5. navigate to SevenPeaks.VTS.Persistence  (persistence project folder)
6. run "dotnet ef --startup-project ../SevenPeaks.VTS.Web/ database update --context DatabaseService"
7. You might get an error messsage like "There is already an object named '...' in the database." kindly ignore, it's a bug from entityframework (and I've not figured out how to fix).
8.  now you can run the SevenPeaks.VTS.Web by navigating into it and run "dotnet run"


## Usage

1. https://localhost:5001/swagger/index.html provides API documentation page
2. https://localhost:5001  is the admin page. where other operation would be performed.
  - Create account and click **"Click here to confirm your account"** in order to verify your account.
  - Login with the registerd email address and password
  - Start performing other tasks
  
### Answer to extensibility question
In order to extend the table, ther are veral approaches but choice will be based on the kind of application.
1. The entity can be modified by adding more columns or adding a new table with the new columns which will have a one-to-one relationship with the main table
2. The use of document database would be another alternative.

