UC#1 .NET AI Repository

# Description
This project contains a simple .NET Core API with one endpoint (/countries) for accessing Countries .NET API. The endpoint retrieves info about countries including their names, population and other stats.  
There are a 4 optional parameters you can use with this endpoint: nameFilter, populationFilter, sortBy and limit. The nameFilter parameter filters countries by only returning ones that containthe search string in their common name. The populationFilter parameter filters countries by maximum population (in millions). The sortBy parameter can be either "ascend" or "descend" and sorts the countries by their name in the respective alphabetical order (ascending or descending). The limit parameter limits the number of countries returned.
# How to run
The application can be run from the main entry point by building and running it with Visual Studio. Once the server starts up, it loads a Swagger page in the browser where you can test the /countries GET endpoint to retrieve the data.
# Example usage
- https://localhost:7268/Countries?nameFilter=saint
Returns list of countries with "saint" in their name.
- https://localhost:7268/Countries?populationFilter=1
Returns list of countries with less than 1 million population.
- https://localhost:7268/Countries?nameFilter=saint&populationFilter=1
Returns list of countries with less than 1 million population and "saint" in their name.
- https://localhost:7268/Countries?nameFilter=saint&sortBy=ascend
Returns list of countries with "saint" in their name and sorts them in ascending alphabetical order.
- https://localhost:7268/Countries?nameFilter=saint&sortBy=descend
Returns list of countries with "saint" in their name and sorts them in descending alphabetical order.
- https://localhost:7268/Countries?populationFilter=1&sortBy=ascend
Returns list of countries with less than 1 million population and sorts them in ascending alphabetical order.
- https://localhost:7268/Countries?populationFilter=1&sortBy=descend
Returns list of countries with less than 1 million population and sorts them in descending alphabetical order.
- https://localhost:7268/Countries?populationFilter=1&limit=5
Returns list of the first 5 countries with less than 1 million population.
- https://localhost:7268/Countries?nameFilter=saint&populationFilter=1&limit=5
Returns list of the first 5 countries with less than 1 million population and "saint" in their name.
- https://localhost:7268/Countries?nameFilter=saint&sortBy=ascend&limit=5
Returns list of the first 5 countries with "saint" in their name and sorts them in ascending alphabetical order.
