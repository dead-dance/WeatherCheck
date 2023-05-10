Initial Thoguht:
In the beginning I thought I would save district data in SQL database from the given json. Also I was planning to create stored procedure to fetch Avg Temperature data in Task 1.
Later whole plan changed and I managed to complete the task without having any sort of any database interaction. However, earlier as I have added Database functionalities using EF
Core, I am keeping it for future extensions and scopes.

Project Overview:
This project is developed on ASP.Net Core Web API. On front-end Angular is used. There it can be accessed via Swagger or via Web UI

How to Run:
1. Download the code or clone repository.
2. Open "WeatherCheck-master" folder using Visual Studio Code.
3. To run Backend, press "F5"
4. To run Angular, type in terminal "cd .\client\"
5. Again type in terminal "ng serve --o"
6. After compiling Backend Part, it will open in "https://localhost:5001/"
7. To access Swagger please click https://localhost:5001/swagger
8. After compiling Angular it will open in http://localhost:4200/
9. To access web UI click "http://localhost:4200/weather"

Please be noted: node_modules needs to be placed inside "...\WeatherCheck-master\client" Folder. Due excessive size, it can not be added to source control.
