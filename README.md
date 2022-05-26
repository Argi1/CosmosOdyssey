# Cosmos Odyssey
This project uses docker to run the .net app and a MySQL database

This is an test assignment done for a job interview
The goal was to create a webpage where users can choose their starting and destination planet. Then they can see all the possible routes 
and providers for all those routes. Then the user can select a flight and be forwarded to a page where they can reserv that flight.

All available routes can be seen on the picture below
![Img](https://i.ibb.co/qpsM2LP/beb41fd47cb1a28e66ee3619083dc1a1.png)

The assignment had a webpage that the .NET app checks to get it's pricelist from. That pricelist has a valid until period and after it expires,
the webpage will update and this .NET app will request new data from it. After 15 updates it will start deleting the oldest pricelists so that there
is always 15 pricelists.

## Getting Started

### Docker setup

#### Requirements
* [Docker & Docker-compose](https://docs.docker.com/compose/install/) 
#### Set-up
1. Clone the repository
2. On command line navigate to the cloned repository
3. Run docker-compose to launch the .NET app and MySql database
   ```sh
   docker-compose up -d
   ```
4. Page should now be up and running. Accessible at http://localhost:8000
 
#### Finishing up
 * To shutdown the running container:
    ```sh
    docker-compose down
    ```
 * To start the container back up, use:
    ```sh
    docker-compose up -d
    ```
### IDE setup with IIS Express and local MySql server
If you want to run this project without docker you can use the IIS Express provided by Visual Studio and a local MySql database.

1. Clone the repository
2. Change the database connection in Data/DatabaseContext.cs to connect to your local MySql database (Example given in comment)
3. Start up your local database
4. Run the project using IIS Express
5. Wait for 20 seconds to let the program create the required tables and fill them with data fetched from the webpage given in the assignment
6. Page should now be fully functional and accessible at http://localhost:8000