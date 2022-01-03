# Cosmos Odyssey
This project uses docker to run the .net app and a MySQL database

This is an test assignment done for a job interview
The goal was to create a webpage where users can choose their starting and destination planet. Then they can see all the possible routes 
and providers for all those routes. Then the user can select a flight and be forwarded to a page where they can reserv that flight.

All available routes can be seen on the picture below
![Route Image](https://imgur.com/a/7kb30jk)

The assignment had a webpage that the .NET app checks to get it's pricelist from. That pricelist has a valid until period and after it expires,
the webpage will update and this .NET app will request new data from it. After 15 updates it will start deleting the oldest pricelists so that there
is always 15 pricelists.

## Getting Started

### Requirements
* [Docker & Docker-compose](https://docs.docker.com/compose/install/) 
### Set-up
1. Clone the repository
2. On command line navigate to the cloned repository
3. Run docker-compose to launch the .NET app and MySql database
   ```sh
   docker-compose up -d
   ```
4. Page should now be up and running. Accessible at http://localhost:8000
 
### Finishing up
 * To shutdown the running container:
    ```sh
    docker-compose down
    ```
 * To start the container back use:
    ```sh
    docker-compose up -d
    ```