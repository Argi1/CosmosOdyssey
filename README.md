# Cosmos Odyssey
This project uses docker to run the .net app and a MySQL database

## Getting Started

### Requirements
* [Docker & Docker-compose](https://docs.docker.com/compose/install/) 
### Set-up
1. Clone the repository
2. On command line navigate to the cloned repository
3. Install dotnet-ef
    ```sh
    dotnet new tool-manifest
    dotnet tool install --local dotnet-ef
    ```
4. Run docker-compose to launch the MySQL database
   ```sh
   docker-compose up -d db
   ```
5. Run database migration and update commands to setup the databse (one time only)
   ```sh
   dotnet ef migrations add Initial
   dotnet ef database update
   ```
6. Change the connection string in ..\CosmosOdyssey\Data\DatabaseContext.cs

   From
   ```sh
   optionsBuilder.UseMySQL("Server = 127.0.0.1; Database = db; Uid = user; Pwd = password;");
   ```
   To
   ```sh
   optionsBuilder.UseMySQL("Server = db; Database = db; Uid = user; Pwd = password;");
   ```
7. Run docker-compose again to launch the .NET app
   ```sh
   docker-compose up -d
   ```
8. Page should now be up and running. Accessible at http://localhost:8000
 
### Finishing up
 * To shutdown the running container:
    ```sh
    docker-compose down
    ```
 * To start the container back use:
    ```sh
    docker-compose up -d
    ```