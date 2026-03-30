Simple .Net 10 WEB API application, which accepts base64 encoded binary data. Id is part of the route, while body contains the data string itself.
The third and the only Get method returns the result.

Application was written using programming principles like SOLID and SOC - and common patterns like Repositories, services and Dependency Injection.

I also created 2 types of tests; integration and unit tests. Integration test the API availability and unit tests test the backend logic itself.

The projects within solution are separated between Api, Business and Tests.
