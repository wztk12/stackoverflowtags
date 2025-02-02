StackOverflowTags
=================

Opis
----

Jest to aplikacja napisana w C#, która wykorzystuje **Serilog** do logowania, **SQLite** jako bazę danych oraz **Swagger** do dokumentacji API. Zawiera **testy jednostkowe i integracyjne**, a jej uruchomienie jest możliwe za pomocą **Docker Compose**. Aplikacja działa wtedy na porcie **5000**.

Funkcjonalności
---------------

*   **Logowanie** – Serilog zapisuje logi do pliku tekstowego.
    
*   **Baza danych** – SQLite jako system bazodanowy.
    
*   **Swagger** – Dokumentacja API dostępna w interfejsie Swagger UI.
    
*   **Testy** – W projekcie znajdują się **testy jednostkowe** i **integracyjne**.
    
*   **Docker** – Aplikację można uruchomić w kontenerze Docker.
       

Uruchamianie aplikacji
----------------------

Aby uruchomić aplikację, należy wykonać polecenie:

```bash
docker-compose up
```

Dokumentacja API
----------------

Po uruchomieniu aplikacji interfejs Swagger UI można znaleźć pod adresem:

http://localhost:5000/swagger

Logowanie
---------

Serilog zapisuje logi w pliku **logs{data}.txt**.

