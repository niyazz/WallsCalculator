# WallsCalculator
Website for calculating building materials for the construction of house walls:
1. Calculation of brick walls
2. Calculation of timber walls
3. Calculation of concrete block walls

For calculations, you need to go to the service section and select the building material you are interested in.<br/>
As a result of entering input data and pressing the calculation button, the calculation results are tabulated.<br/>
The results table can be uploaded to ``Microsoft Word (.doc)``.

## Tools and technologies
- ASP.NET Core MVC 5
- Bootstrap v4.3.1
- HTML, CSS
- JQuery cleint-side validation for ASP.NET

## Launching the app
There are several options for launching the application with and without using ``Docker``:

### Using a ready-made image
```bash
# download image from docker-hub
docker pull niyazz/wallscalculator  

# running a container named walls_calculator in detached mode with auto-deletion
docker run --name walls_calculator --rm -d -p 8080:80 niyazz/wallscalculator 

#visit http://localhost:8080/
```

### Creating an image and run container
```bash
# cloning a repository in src
https://github.com/niyazz/WallsCalculator.git 
cd WallsCalculator/WallsCalculator

# build an image
docker build -t name_of_your_image .  

# running a container named walls_calculator in detached mode with auto-deletion
docker run --name walls_calculator --rm -d -p 8080:80 name_of_your_image 

#visit http://localhost:8080/
```

### Without using Docker
```bash
# build an image 
https://github.com/niyazz/WallsCalculator.git 
cd WallsCalculator/WallsCalculator

# restore project and run app
dotnet restore
dotnet run

#visit https://localhost:7139/
```

