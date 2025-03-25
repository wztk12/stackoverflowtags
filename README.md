StackOverflowTags
=================

Description
----

This is an application written in C# that uses **Serilog** for logging, **SQLite** as a database, and **Swagger** for API documentation. It contains **unit and integration tests**, and can be run using **Docker Compose**. The application then runs on port **5000**.

Functionalities
----------------

* **Logging** – Serilog saves logs to a text file.

* **Database** – SQLite as a database system.

* **Swagger** – API documentation available in the Swagger UI.

* **Tests** – The project contains **unit** and **integration** tests.

* **Docker** – The application can be run in a Docker container.

Running the application
---------------------

To run the application, execute the command:

```bash
docker-compose up
```

API Documentation
----------------

After running the application, the Swagger UI can be found at:

http://localhost:5000/swagger

Logging
---------

Serilog saves logs to the file **logs{data}.txt**.
