# TransactionManagement

This project was developed as part of a test task. A detailed instruction is also attached to it below.

# **Project Structure:**

1. TransactionManagement.BLL: This contains the interfaces and the services that implement them.
2. TransactionManagement.DAL: This describes the structure of the database, created using Entity Framework (EF).
3. TransactionManagement.DTO: This contains the database models for easier handling in the code.
4. TransactionManagement.Tests: This contains the unit tests.
5. webapi: This is the server, where the controller and all project settings are implemented.
   
![image](https://github.com/user-attachments/assets/09bbbb18-3b8d-41d8-95ab-f8e180548ca2)

# **Implementation**

In this task, the following functionality has been implemented:
1. After uploading the file, .NET processes the content and adds the data to the database based on transaction_id found in the CSV file. If no record with such a unique transaction_id exists in the database, a new record is added; if a record does exist, the transaction status is updated.
2. The client's and transaction's time zones can be determined from the location coordinates. Any libraries or online services can be used for this conversion.
3. When exporting to Excel, the user should be able to download a file with transaction information (columns chosen by the developer).
4. Allow the user to retrieve a list of transactions within a date range that occurred in the time zone of the current user making the API requests.
5. Allow the user to retrieve a list of transactions within a date range that occurred in the clients' time zones. The client's time zone is stored with each transaction and was obtained from the geolocation of the specific transaction.
6. Allow the user to retrieve a list of transactions for January 2024 that occurred in the clients' time zones.

# **How to Use?**

First, edit the path to the database and perform a migration.
Let’s break down the methods:
The upload-csv method in the Transactions controller works as follows:
First, upload a file with a .csv extension. If the file has a different extension, the program will notify you and return an error. After that, the current transaction ID is checked against the IDs in the database. If there is a match, the fields are updated; if not, new records are created. To avoid overloading the GeoNamesApi, which, if I’m not mistaken, allows 250 requests, it was decided to check if UTC has a "Not found time" record. If so, we make an API call to update the data. We use UTC instead of IANAZone because, as I understand it, many coordinates fall into the ocean, where there are no settlements, making it difficult to determine the zone; you’d need to find the nearest settlement, or it might be an API shortcoming.
How does the Location service work? We pass the coordinates, and then calculations are performed using the time zone and zone API.
The next method is get-transactions, which is used to fulfill task 4. The user must specify an identifier, which could be a transaction ID, email, or name. Then they specify two dates to search within and, optionally, list the columns to download in the file. If the user doesn’t need to download the data in a file, they can change the exportToExcel value from true to false, and all results will be returned in the body. If the user wants to download an Excel file, they need to list the columns in the following format:
"selectedColumns": [
  "TransactionId",
  "Name",
  "Email",
  "Amount",
  "TransactionDate",
  "IANAZone",
  "UTC",
  "Coordinates"
],
They can leave any field of their choice.
Let’s consider the next method, get-transactions-by-date, which implements task 5. The user must fill in two dates and, similarly to the previous section, use Excel download.
The final method is get-transactions-for-january-2024, which implements task 6. The user must enter their identifier and, as mentioned above, use Excel.

# **Tests**

Two tests were implemented for the two services, namely Transaction and Location, to check how the methods work. These are my first tests, so I’m not entirely sure if they are written correctly, but this is how it turned out.
