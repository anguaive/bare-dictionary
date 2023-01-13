## dictionary-api

Only needed a single `translate` endpoint, so I went with a single project and Minimal APIs for simplicity. Database schema is set up using an EF Core migration. 

In development mode, the database is seeded from the provided spreadsheet (converted to .csv) at startup using CsvHelper, if the schema is up-to-date.

TODO: testing

